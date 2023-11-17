
namespace mini_compiler;
#region PredefineFunctionClass
/// <summary>
/// Represents a function with a name, number of parameters, and implementation.
/// </summary>
public class Functions
{
    public string Name { get; }
    public int Parameters { get; }
    public Func<List<object>, object> Implementation { get; }
    public Functions(string name, int parameters, Func<List<object>, object> implementation)
    {
        Name = name;
        Parameters = parameters;
        Implementation = implementation;
    }
}
#endregion
#region DeclarableFunctionClass
/// <summary>
/// Represents a function in the program.
/// </summary>
public class DFunction : Token
{
    public static readonly List<DFunction> functions = new();
    public List<Token> expression;
    public List<string> parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="DFunction"/> class.
    /// </summary>
    /// <param name="expression">The expression that defines the function.</param>
    /// <param name="value">The name of the function.</param>
    /// <param name="parameters">The list of parameters for the function.</param>
    /// <param name="type">The return type of the function.</param>
    public DFunction(List<Token> expression, string value, List<string> parameters, TokenType type) : base(type, value, 0, 0)
    {
        this.expression = expression;
        expression.Add(new Token(TokenType.EOL, ";", 0, 0)); // Assuming 0, 0 as line and column numbers, this works suprisingly well
        this.value = value;
        this.parameters = parameters;
        this.type = type;
        functions.Add(this); // Add this instance of DFunction to the functions list
    }
}
#endregion
#region Interpreter_Function_Eval
public partial class Interpreter
{
    short StackOverFlowCounter = 0;
    /// <summary>
    /// Evaluates a function with the given arguments by substituting the parameters in the function expression and evaluating the resulting expression.
    /// </summary>
    /// <param name="fn">The function to evaluate.</param>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns>The result of evaluating the function expression with the substituted arguments.</returns>
    private object EvaluateFunction(DFunction fn, List<object> args) // This one used to be called "EvaFurras", but my conscience prevented me from leaving it that way.
    {
        StackOverFlowCounter++;
        if (StackOverFlowCounter >= 2000)
        {
            Console.WriteLine("StackOverFlow Exception, more than 2000 iterations");
            throw new Exception();
        }
        var expressionTokens = fn.expression;
        var parameterList = fn.parameters;

        if (args.Count != parameterList.Count)
        {
            Console.WriteLine($"Expected {parameterList.Count} arguments, but got {args.Count}");
            return null!;
        }

        var argDict = new Dictionary<string, object>();
        for (int i = 0; i < parameterList.Count; i++)
        {
            argDict[parameterList[i]] = args[i];
        }

        var substitutedExpression = SubstituteArgs(expressionTokens, argDict);
        return Expression(substitutedExpression);
    }
    /// <summary>
    /// Substitutes the arguments in a list of tokens with their corresponding values from a dictionary.
    /// </summary>
    /// <param name="tokens">The list of tokens to substitute arguments in.</param>
    /// <param name="argDict">The dictionary containing the argument names and their corresponding values.</param>
    /// <returns>A new list of tokens with the arguments substituted.</returns>
    private static List<Token> SubstituteArgs(List<Token> tokens, Dictionary<string, object> argDict)
    {
        var substitutedTokens = new List<Token>();

        foreach (var token in tokens)
        {
            if (token.type == TokenType.Identifier && argDict.ContainsKey(token.value))
            {
                var argValue = argDict[token.value];
                TokenType argType;

                // Determine the type of the argument
                if (argValue is float)
                {
                    argType = TokenType.Number;
                }
                else if (argValue is string)
                {
                    argType = TokenType.StringLiteral;
                }
                else
                {
                    // Handle other types as needed
                    argType = TokenType.Identifier;
                }

                substitutedTokens.Add(new Token(argType, argValue.ToString()!, token.line, token.column));
            }
            else
            {
                substitutedTokens.Add(token);
            }
        }

        return substitutedTokens;
    }
    // This is the equivalent to EvaluateFunction in other "projects" I guess
    #region ExpressionOverride
    /// <summary>
    /// Parses and evaluates an expression from a list of tokens.
    /// </summary>
    /// <param name="tokens">The list of tokens to parse.</param>
    /// <returns>The result of the evaluated expression.</returns>
    private object Expression(List<Token> tokens)
    {
        var left = Term(tokens);

        while (tokens.Count > 0)
        {
            var token = tokens[0];
            tokens.RemoveAt(0);

            // Verificar si el operador es '@'
            if (token.type == TokenType.Operator && token.value == "@")
            {
                var right = Term(tokens);
                left = ConcatenateValues(left, right);
            }
            else if (token.type == TokenType.Operator && (token.value == "+" || token.value == "-"))
            {
                var right = Term(tokens);
                left = BinaryOperation(left, token, right);
            }
            else
            {
                // Si no es un operador, devolver el token a la lista para que pueda ser procesado en la siguiente iteración
                tokens.Insert(0, token);
                return left;
            }
        }

        return left;
    }

    /// <summary>
    /// Evaluates the term expression of the given list of tokens.
    /// </summary>
    /// <param name="tokens">The list of tokens to evaluate.</param>
    /// <returns>The result of the evaluation.</returns>
    private object Term(List<Token> tokens)
    {
        var left = Power(tokens);

        while (tokens.Count > 0)
        {
            var token = tokens[0];

            if (token.type != TokenType.Operator || (token.value != "*" && token.value != "/" && token.value != "%"))
            {
                return left;
            }

            tokens.RemoveAt(0);
            var right = Power(tokens);
            left = BinaryOperation(left, token, right);
        }

        return left;
    }

    /// <summary>
    /// Evaluates the power operation in a list of tokens.
    /// </summary>
    /// <param name="tokens">The list of tokens to evaluate.</param>
    /// <returns>The result of the power operation.</returns>
    private object Power(List<Token> tokens)
    {
        var left = Primary(tokens);

        while (tokens.Count > 0)
        {
            var token = tokens[0];

            if (token.type != TokenType.Operator || token.value != "^")
            {
                return left;
            }

            tokens.RemoveAt(0);
            var right = Primary(tokens);
            left = BinaryOperation(left, token, right);
        }

        return left;
    }

    /// <summary>
    /// Parses and evaluates primary expressions.
    /// </summary>
    /// <param name="tokens">List of tokens to be parsed.</param>
    /// <returns>The result of the evaluated expression.</returns>
    private object Primary(List<Token> tokens)
    {
        var token = tokens[0];
        tokens.RemoveAt(0);

        var function = functions?.Find(func => func.value == token.value);
        if (function != null)
        {
            List<object> args = new();
            var nextToken = tokens[0];
            tokens.RemoveAt(0);
            if (nextToken.value != "(" || nextToken.type != TokenType.Punctuation)
            {
                Console.WriteLine($"Expected \"(\" after function name at {nextToken.column}");
            }
            while (tokens.Count > 0)
            {
                var arg = Expression(tokens);
                args.Add(arg);
                var delimiterToken = tokens[0];
                tokens.RemoveAt(0);
                if (delimiterToken.value == ")")
                {
                    break;
                }
                else if (delimiterToken.value != "," || delimiterToken.type != TokenType.Punctuation)
                {
                    Console.WriteLine($"Expected \",\" or \")\" after function argument at {delimiterToken.column}");
                }
            }
            return EvaluateFunction(function, args);
        }
        var function2 = functions2?.Find(func => func.Name == token.value);
        if (function2 != null)
        {
            List<object> args = new();
            var nextToken = tokens[0];
            tokens.RemoveAt(0);
            if (nextToken.value != "(" || nextToken.type != TokenType.Punctuation)
            {
                Console.WriteLine($"Expected \"(\" after function name at {nextToken.column}");
            }
            while (tokens.Count > 0)
            {
                var arg = Expression(tokens);
                args.Add(arg);
                var delimiterToken = tokens[0];
                tokens.RemoveAt(0);
                if (delimiterToken.value == ")")
                {
                    break;
                }
                else if (delimiterToken.value != "," || delimiterToken.type != TokenType.Punctuation)
                {
                    Console.WriteLine($"Expected \",\" or \")\" after function argument at {delimiterToken.column}");
                }
            }
            return function2.Implementation(args);
        }
        else if (constants.ContainsKey(token.value))
        {
            // If the token is a constant, return its value
            return constants[token.value];
        }
        if (token.type == TokenType.Number)
        {
            return float.Parse(token.value);
        }
        else if (token.type == TokenType.StringLiteral)
        {
            return token.value;
        }
        else if (token.type == TokenType.Identifier)
        {
            if (variables.ContainsKey(token.value))
            {
                return variables[token.value];
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Undefined variable '{token.value}' at line {token.line} and column {token.column}");
            throw new Exception(); //Don't ask me why i did this, I'm desesperado.
        }
        else if (token.type == TokenType.Punctuation && token.value == "(")
        {
            var expressionValue = Expression(tokens);
            variables.Clear();
            var nextToken = tokens[0];
            tokens.RemoveAt(0);
            if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
            {
                Console.WriteLine($"Expected ')' after expression at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
            return expressionValue;
        }
        else if (token.type == TokenType.LetKeyword && token.value == "let")
        {
            var variableToken = tokens[0];
            tokens.RemoveAt(0);
            if (variableToken.type != TokenType.Identifier)
            {
                Console.WriteLine($"Expected identifier after 'let' keyword at line {variableToken.line} and column {variableToken.column}");
                return null!;
            }
            var equalsToken = tokens[0];
            tokens.RemoveAt(0);
            if (equalsToken.type != TokenType.Operator || equalsToken.value != "=")
            {
                Console.WriteLine($"Expected '=' after identifier in 'let' expression at line {equalsToken.line} and column {equalsToken.column}");
                return null!;
            }
            var value = Expression(tokens);
            variables[variableToken.value] = value;
            var nextToken = tokens[0];
            tokens.RemoveAt(0);
            if (nextToken.type != TokenType.InKeyword || nextToken.value != "in")
            {
                Console.WriteLine($"Expected 'in' keyword after expression in 'let' expression at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
            var expressionValue = Expression(tokens);
            return expressionValue;
        }
        else if (token.type == TokenType.IfKeyword)
        {
            return ReturnConditionalValue(tokens);
        }
        else if (token.type == TokenType.Operator && token.value == "-")
        {
            bool isNegative = true;
            var nextToken = tokens[0];
            while (nextToken.type == TokenType.Operator && nextToken.value == "-")
            {
                isNegative = !isNegative;
                tokens.RemoveAt(0);
                nextToken = tokens[0];
            }
            if (nextToken.type == TokenType.Number)
            {
                return (isNegative ? -1 : 1) * float.Parse(nextToken.value);
            }
            else if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
            {
                var expressionValue = Expression(tokens);
                tokens.RemoveAt(0);
                nextToken = tokens[0];
                tokens.RemoveAt(0);  // remove the closing parenthesis
                return (isNegative ? -1 : 1) * (float)expressionValue;
            }
            else
            {
                Console.WriteLine($"Expected number or '(' after '-' operator at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
        }
        else
        {
            Console.WriteLine($"Invalid expression at line {token.line} and column {token.column}");
            return null!;
        }
    }
    #endregion
    #region RConditionalOverload
    /// <summary>
    /// Evaluates a conditional statement and returns the result of the corresponding block of code.
    /// </summary>
    /// <param name="tokens">The list of tokens representing the conditional statement.</param>
    /// <returns>The result of the corresponding block of code if the condition is true, otherwise null.</returns>
    private object ReturnConditionalValue(List<Token> tokens)
    {
        var token = tokens[0];
        tokens.RemoveAt(0);
        if (token.type == TokenType.Punctuation && token.value == "(")
        {
            var leftValue = Expression(tokens);
            var comparisonToken = tokens[0];
            tokens.RemoveAt(0);
            if (comparisonToken.type != TokenType.ComparisonOperator)
            {
                Console.WriteLine($"Expected comparison operator after left-hand side of comparison in 'if' statement at line {comparisonToken.line} and column {comparisonToken.column}");
                return null!;
            }
            var rightValue = Expression(tokens);
            var nextToken = tokens[0];
            tokens.RemoveAt(0);
            if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
            {
                Console.WriteLine($"Expected ')' after right-hand side of comparison in 'if' statement at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
            var comparisonOperator = comparisonToken.value;
            bool comparisonResult;
            switch (comparisonOperator)
            {
                case "<":
                    comparisonResult = (float)leftValue < (float)rightValue;
                    break;
                case ">":
                    comparisonResult = (float)leftValue > (float)rightValue;
                    break;
                case "<=":
                    comparisonResult = (float)leftValue <= (float)rightValue;
                    break;
                case ">=":
                    comparisonResult = (float)leftValue >= (float)rightValue;
                    break;
                case "==":
                    comparisonResult = leftValue.Equals(rightValue);
                    break;
                case "!=":
                    comparisonResult = !leftValue.Equals(rightValue);
                    break;
                default:
                    Console.WriteLine($"Invalid comparison operator '{comparisonOperator}' in 'if' statement at line {comparisonToken.line} and column {comparisonToken.column}");
                    return null!;
            }
            if (comparisonResult)
            {
                var result = Expression(tokens);
                // Skip the else block
                while (true)
                {
                    token = tokens[0];
                    tokens.RemoveAt(0);
                    if (token.type == TokenType.InKeyword)
                    {
                        tokens.Insert(0, token); // Put the InKeyword back into the tokens
                        break;
                    }
                    else if (token.type == TokenType.EOL)
                    {
                        break;
                    }
                }
                return result;
            }
            else
            {
                // Skip to the else
                while (token.type != TokenType.ElseKeyword && token.type != TokenType.EOL)
                {
                    token = tokens[0];
                    tokens.RemoveAt(0);
                }
                if (token.type == TokenType.ElseKeyword)
                {
                    return Expression(tokens);
                }
                else
                {
                    Console.WriteLine($"Expected 'else' keyword at line {token.line} and column {token.column}");
                    return null!;
                }
            }
        }
        else if (token.type == TokenType.Identifier)
        {
            var function = functions?.Find(func => func.value == token.value);
            if (function != null)
            {
                List<object> args = new();
                var nextToken = tokens[0];
                tokens.RemoveAt(0);
                if (nextToken.value != "(" || nextToken.type != TokenType.Punctuation)
                {
                    Console.WriteLine($"Expected \"(\" after function name at {nextToken.column}");
                }
                while (tokens.Count > 0)
                {
                    var arg = Expression(tokens);
                    args.Add(arg);
                    var delimiterToken = tokens[0];
                    tokens.RemoveAt(0);
                    if (delimiterToken.value == ")")
                    {
                        break;
                    }
                    else if (delimiterToken.value != "," || delimiterToken.type != TokenType.Punctuation)
                    {
                        Console.WriteLine($"Expected \",\" or \")\" after function argument at {delimiterToken.column}");
                    }
                }
                return EvaluateFunction(function, args);
            }
            else
            {
                Console.WriteLine($"Undefined function '{token.value}' at line {token.line} and column {token.column}");
                return null!;
            }
        }
        else
        {
            Console.WriteLine($"Expected '(' or function name after 'if' keyword at line {token.line} and column {token.column}");
            return null!;
        }
    }
    #endregion
}
#endregion
#region Lexer_FunctionDeclaration
public partial class Lexer
{
    /// <summary>
    /// Process the assignment of declarable? functions and add them to the list of functions
    /// </summary>
    /// <returns> DFunction </returns>
    private Token FunctionDeclaration()
    {
        // Check that the next token is 'function'
        // After 'function' we expect an identifier
        Advance(); // Advance to the next token
        SkipWhitespace(); // Skip all the whitespace characters until a non-whitespace character is found

        if (!char.IsLetter(current_char))
        {
            // If the next character is not a letter, print an error
            Console.WriteLine($"Expected a function name at line {line} and column {column}");
        }
        //Alloc? corresponding name
        string function_name = "";
        // While non whitespace characters are found, append them to the function_name string
        while (current_char != '\0' && char.IsLetterOrDigit(current_char))
        {
            function_name += current_char;
            Advance();
        }
        SkipWhitespace();
        if (current_char != '(')
        {
            // If the next character is not a '(', print an error
            Console.WriteLine($"Expected '(' after function name at line {line} and column {column}");
        }

        // Check the parameters of the function
        List<string> parameters = new();
        Advance();
        while (current_char != ')')
        {
            if (!char.IsLetter(current_char))
            {
                // If the next character is not a letter, print an error
                Console.WriteLine($"Expected a parameter name at line {line} and column {column}");
            }

            string parameter_name = "";
            while (current_char != '\0' && char.IsLetterOrDigit(current_char))
            {
                // While non whitespace characters are found, append them to the parameter_name string
                parameter_name += current_char;
                Advance();
            }
            // Add the parameter name to the list of parameters
            parameters.Add(parameter_name);
            SkipWhitespace();
            // Multi parameter support
            if (current_char == ',')
            {
                Advance();
                SkipWhitespace();
            }
        }
        Advance(); // Move forward after ')'
        var nextToken = GetNextToken();
        if (nextToken.type != TokenType.FLinq || nextToken.value != "=>")
        {
            // If the next token is not '=>', print an error
            Console.WriteLine($"Expected '=>' after function parameters at line {line} and column {column}");
        }
        // Analize the function body as an expression
        var expressionTokens = new List<Token>();
        nextToken = GetNextToken();
        // While the next token is not a semicolon, add the tokens to the expressionTokens list
        // This gave a funny overflow once, ahh hilarious
        while (nextToken.type != TokenType.Semicolon && nextToken.type != TokenType.EOF)
        {
            expressionTokens.Add(nextToken);
            nextToken = GetNextToken();
        }
        // Returns a new DFunction with the expressionTokens, function_name, parameters and TokenType.FunctionDeclaration
        return new DFunction(expressionTokens, function_name, parameters, TokenType.FunctionDeclaration);
    }
}
#endregion