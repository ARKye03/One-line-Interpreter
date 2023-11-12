
namespace mini_compiler;

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
public class FunctionToken : Token
{
    public static List<DFunction> functions = new();
    public List<string> Parameters { get; }

    public FunctionToken(TokenType type, string value, int line, int column, List<string> parameters, List<Token> expression)
        : base(type, value, line, column)
    {
        Parameters = parameters;
        expression.RemoveAt(expression.Count - 1);
        functions.Add(new DFunction(expression, value, parameters, type));
    }
}
public class DFunction
{
    public List<Token> expression;
    public string value;
    public List<string> parameters;
    public TokenType type;
    public DFunction(List<Token> expression, string value, List<string> parameters, TokenType type)
    {
        this.expression = expression;
        expression.Add(new Token(TokenType.EOL, ";", 0, 0));
        this.value = value;
        this.parameters = parameters;
        this.type = type;
    }
}

public partial class Interpreter
{
    private object EvaFurras(DFunction fn, List<object> args)
    {
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
        return expression(substitutedExpression);
    }
    // This substitute parameters for arguments
    private List<Token> SubstituteArgs(List<Token> tokens, Dictionary<string, object> argDict)
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
    private object expression(List<Token> tokens)
    {
        var left = term(tokens);

        while (tokens.Count > 0)
        {
            var token = tokens[0];
            tokens.RemoveAt(0);

            // Verificar si el operador es '@'
            if (token.type == TokenType.Operator && token.value == "@")
            {
                var right = term(tokens);
                left = ConcatenateValues(left, right);
            }
            else if (token.type == TokenType.Operator && (token.value == "+" || token.value == "-"))
            {
                var right = term(tokens);
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

    private object term(List<Token> tokens)
    {
        var left = power(tokens);

        while (tokens.Count > 0)
        {
            var token = tokens[0];

            if (token.type != TokenType.Operator || (token.value != "*" && token.value != "/" && token.value != "%"))
            {
                return left;
            }

            tokens.RemoveAt(0);
            var right = power(tokens);
            left = BinaryOperation(left, token, right);
        }

        return left;
    }

    private object power(List<Token> tokens)
    {
        var left = primary(tokens);

        while (tokens.Count > 0)
        {
            var token = tokens[0];

            if (token.type != TokenType.Operator || token.value != "^")
            {
                return left;
            }

            tokens.RemoveAt(0);
            var right = primary(tokens);
            left = BinaryOperation(left, token, right);
        }

        return left;
    }

    private object primary(List<Token> tokens)
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
                var arg = expression(tokens);
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
            return EvaFurras(function, args);
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
                var arg = expression(tokens);
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
            Console.WriteLine($"Undefined variable '{token.value}' at line {token.line} and column {token.column}");
            return null!;
        }
        else if (token.type == TokenType.Punctuation && token.value == "(")
        {
            var expressionValue = expression(tokens);
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
            var value = expression(tokens);
            variables[variableToken.value] = value;
            var nextToken = tokens[0];
            tokens.RemoveAt(0);
            if (nextToken.type != TokenType.InKeyword || nextToken.value != "in")
            {
                Console.WriteLine($"Expected 'in' keyword after expression in 'let' expression at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
            var expressionValue = expression(tokens);
            return expressionValue;
        }
        else if (token.type == TokenType.IfKeyword)
        {
            return RConditional(tokens);
        }
        else if (token.type == TokenType.Operator && token.value == "-")
        {
            var nextToken = tokens[0];
            tokens.RemoveAt(0);
            if (nextToken.type == TokenType.Number)
            {
                return -float.Parse(nextToken.value);
            }
            Console.WriteLine($"Expected number after '-' operator at line {nextToken.line} and column {nextToken.column}");
            return null!;
        }
        else
        {
            Console.WriteLine($"Invalid expression at line {token.line} and column {token.column}");
            return null!;
        }
    }
    #endregion
    #region RConditionalOverload
    private object RConditional(List<Token> tokens)
    {
        var token = tokens[0];
        tokens.RemoveAt(0);
        if (token.type == TokenType.Punctuation && token.value == "(")
        {
            var leftValue = expression(tokens);
            var comparisonToken = tokens[0];
            tokens.RemoveAt(0);
            if (comparisonToken.type != TokenType.ComparisonOperator)
            {
                Console.WriteLine($"Expected comparison operator after left-hand side of comparison in 'if' statement at line {comparisonToken.line} and column {comparisonToken.column}");
                return null!;
            }
            var rightValue = expression(tokens);
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
                var result = expression(tokens);
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
                    return expression(tokens);
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
                    var arg = expression(tokens);
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
                return EvaFurras(function, args);
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

public partial class Lexer
{
    /// <summary>
    /// Process the assignment of declarable? functions and add them to the list of functions
    /// </summary>
    /// <returns> DFunction </returns>
    private Token function_declaration()
    {
        // Check that the next token is 'function'
        // After 'function' we expect an identifier
        advance(); // Advance to the next token
        skip_whitespace(); // Skip all the whitespace characters until a non-whitespace character is found

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
            advance();
        }
        skip_whitespace();
        if (current_char != '(')
        {
            // If the next character is not a '(', print an error
            Console.WriteLine($"Expected '(' after function name at line {line} and column {column}");
        }

        // Check the parameters of the function
        List<string> parameters = new List<string>();
        advance();
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
                advance();
            }
            // Add the parameter name to the list of parameters
            parameters.Add(parameter_name);
            skip_whitespace();
            // Multi parameter support
            if (current_char == ',')
            {
                advance();
                skip_whitespace();
            }
        }
        advance(); // Avanzar después del ')'
        var nextToken = get_next_token();
        if (nextToken.type != TokenType.FLinq || nextToken.value != "=>")
        {
            // If the next token is not '=>', print an error
            Console.WriteLine($"Expected '=>' after function parameters at line {line} and column {column}");
        }
        // Analize the function body as an expression
        var expressionTokens = new List<Token>();
        nextToken = get_next_token();
        // While the next token is not a semicolon, add the tokens to the expressionTokens list
        // This gave a funny overflow once, ahh hilarious
        while (nextToken.type != TokenType.Semicolon && nextToken.type != TokenType.EOF)
        {
            expressionTokens.Add(nextToken);
            nextToken = get_next_token();
        }
        // Return the function token, then it turns into a DFunction, that can be highly impproved, but for now, I need this to be like this, for me
        return new FunctionToken(TokenType.FunctionDeclaration, function_name, line, column, parameters, expressionTokens);
    }
}