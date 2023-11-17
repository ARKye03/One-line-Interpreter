namespace mini_compiler;
public partial class Interpreter
{
    /*Function that parses expressions and returns their value
     *term(), power() and primary() are functions that parse, respectively, terms, powers and primaries
     *expression() is the function that parses expressions
     *For the functions section, a list of functions is used, in addition to these same functions but overloaded and adapted to another environment
     */
    /// <summary>
    /// Parses an expression and returns the result of the evaluation.
    /// </summary>
    /// <returns>The result of the evaluation.</returns>
    private object Expression()
    {
        var left = Term();

        while (true)
        {
            var token = lexer.GetNextToken();

            // Check if the operator is '@'
            if (token.type == TokenType.Operator && token.value == "@")
            {
                var right = Term();
                left = ConcatenateValues(left, right);
            }
            else if (token.type == TokenType.Operator && (token.value == "+" || token.value == "-"))
            {
                var right = Term();
                left = BinaryOperation(left, token, right);
            }
            else
            {
                // If not an operator, return the token to the lexer so it can be processed in the next iteration
                lexer.UngetToken(token);
                return left;
            }
        }
    }
    /// <summary>
    /// Parses and evaluates a term in the expression, which is a sequence of factors separated by multiplication, division, or modulo operators.
    /// </summary>
    /// <returns>The result of the evaluated term.</returns>
    private object Term()
    {
        var left = Power();

        while (true)
        {
            var token = lexer.GetNextToken();

            if (token.type != TokenType.Operator || (token.value != "*" && token.value != "/" && token.value != "%"))
            {
                lexer.UngetToken(token);
                return left;
            }
            var right = Power();
            left = BinaryOperation(left, token, right);
        }
    }
    /// <summary>
    /// Parses and evaluates power expressions.
    /// </summary>
    /// <returns>The result of the power expression.</returns>
    private object Power()
    {
        var left = Primary();

        while (true)
        {
            var token = lexer.GetNextToken();

            if (token.type != TokenType.Operator || token.value != "^")
            {
                lexer.UngetToken(token);
                return left;
            }
            var right = Primary();
            left = BinaryOperation(left, token, right);
        }
    }
    /// <summary>
    /// Parses primary expressions in the input code.
    /// </summary>
    /// <returns>The parsed object.</returns>
    private object Primary()
    {
        var token = lexer.GetNextToken();

        var function = functions?.Find(func => func.value == token.value);
        if (function != null)
        {
            List<object> args = new();
            var nextToken = lexer.GetNextToken();
            if (nextToken.value != "(" || nextToken.type != TokenType.Punctuation)
            {
                Console.WriteLine($"Expected \"(\" after function name at {nextToken.column}");
            }
            while (true)
            {
                var arg = Expression();
                args.Add(arg);
                var delimiterToken = lexer.GetNextToken();
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
        var functionx = functions2?.Find(func => func.Name == token.value);
        if (functionx != null)
        {
            List<object> args = new();
            var nextToken = lexer.GetNextToken();
            if (nextToken.value != "(" || nextToken.type != TokenType.Punctuation)
            {
                Console.WriteLine($"Expected \"(\" after function name at {nextToken.column}");
            }
            while (true)
            {
                var arg = Expression();
                args.Add(arg);
                var delimiterToken = lexer.GetNextToken();
                if (delimiterToken.value == ")")
                {
                    break;
                }
                else if (delimiterToken.value != "," || delimiterToken.type != TokenType.Punctuation)
                {
                    Console.WriteLine($"Expected \",\" or \")\" after function argument at {delimiterToken.column}");
                }
            }
            // Call the function with the arguments
            return functionx.Implementation(args);
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
            var expressionValue = Expression();
            variables.Clear();
            var nextToken = lexer.GetNextToken();
            if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
            {
                Console.WriteLine($"Expected ')' after expression at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
            return expressionValue;
        }
        else if (token.type == TokenType.LetKeyword && token.value == "let")
        {
            var variableToken = lexer.GetNextToken();
            if (variableToken.type != TokenType.Identifier)
            {
                Console.WriteLine($"Expected identifier after 'let' keyword at line {variableToken.line} and column {variableToken.column}");
                return null!;
            }
            var equalsToken = lexer.GetNextToken();
            if (equalsToken.type != TokenType.Operator || equalsToken.value != "=")
            {
                Console.WriteLine($"Expected '=' after identifier in 'let' expression at line {equalsToken.line} and column {equalsToken.column}");
                return null!;
            }
            var value = Expression();
            variables[variableToken.value] = value;
            var nextToken = lexer.GetNextToken();
            if (nextToken.type != TokenType.InKeyword || nextToken.value != "in")
            {
                Console.WriteLine($"Expected 'in' keyword after expression in 'let' expression at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
            var expressionValue = Expression();
            return expressionValue;
        }
        else if (token.type == TokenType.IfKeyword && token.value == "if")
        {
            return ReturnConditionalValue();
        }
        else if (token.type == TokenType.Operator && token.value == "-")
        {
            bool isNegative = false;
            while (token.type == TokenType.Operator && token.value == "-")
            {
                isNegative = !isNegative;
                token = lexer.GetNextToken();
            }
            if (token.type == TokenType.Number)
            {
                return (isNegative ? -1 : 1) * float.Parse(token.value);
            }
            else if (token.type == TokenType.Punctuation && token.value == "(")
            {
                var expressionValue = Expression();
                return (isNegative ? -1 : 1) * (float)expressionValue;
            }
            else
            {
                lexer.UngetToken(token);
                var expressionValue = Expression();
                return (isNegative ? -1 : 1) * (float)expressionValue;
            }
            /* Console.WriteLine($"Expected number after '-' operator at line {token.line} and column {token.column}");
            return null!; */
        }
        else
        {
            Console.WriteLine($"Invalid expression at line {token.line} and column {token.column}");
            return null!;
        }
    }
}