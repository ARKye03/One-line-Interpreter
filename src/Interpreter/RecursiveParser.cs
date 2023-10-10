namespace mini_compiler;
public partial class Interpreter
{
    private object expression()
    {
        var left = term();

        while (true)
        {
            var token = lexer.get_next_token();

            // Verificar si el operador es '@'
            if (token.type == TokenType.Operator && token.value == "@")
            {
                var right = term();
                left = ConcatenateValues(left, right);
            }
            else if (token.type == TokenType.Operator && (token.value == "+" || token.value == "-"))
            {
                var right = term();
                left = BinaryOperation(left, token, right);
            }
            else
            {
                // Si no es un operador, devolver el token al lexer para que pueda ser procesado en la siguiente iteración
                lexer.unget_token(token);
                return left;
            }
        }
    }
    private object term()
    {
        var left = power();

        while (true)
        {
            var token = lexer.get_next_token();

            if (token.type != TokenType.Operator || (token.value != "*" && token.value != "/" && token.value != "%"))
            {
                lexer.unget_token(token);
                return left;
            }

            var right = power();
            left = BinaryOperation(left, token, right);
        }
    }
    private object power()
    {
        var left = primary();

        while (true)
        {
            var token = lexer.get_next_token();

            if (token.type != TokenType.Operator || token.value != "^")
            {
                lexer.unget_token(token);
                return left;
            }

            var right = primary();
            left = BinaryOperation(left, token, right);
        }
    }
    private object factor()
    {
        var left = primary();

        while (true)
        {
            var token = lexer.get_next_token();

            if (token.type != TokenType.Operator || (token.value != "+" && token.value != "-"))
            {
                lexer.unget_token(token);
                return left;
            }

            var right = primary();
            left = BinaryOperation(left, token, right);
        }
    }
    private object primary()
    {
        var token = lexer.get_next_token();

        foreach (var func in functions!)
        {
            if (token.value == func.value)
            {
                return func.expression;
            }
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
            var expressionValue = expression();
            var nextToken = lexer.get_next_token();
            if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
            {
                Console.WriteLine($"Expected ')' after expression at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
            return expressionValue;
        }
        else if (token.type == TokenType.LetKeyword && token.value == "let")
        {
            var variableToken = lexer.get_next_token();
            if (variableToken.type != TokenType.Identifier)
            {
                Console.WriteLine($"Expected identifier after 'let' keyword at line {variableToken.line} and column {variableToken.column}");
                return null!;
            }
            var equalsToken = lexer.get_next_token();
            if (equalsToken.type != TokenType.Operator || equalsToken.value != "=")
            {
                Console.WriteLine($"Expected '=' after identifier in 'let' expression at line {equalsToken.line} and column {equalsToken.column}");
                return null!;
            }
            var value = expression();
            variables[variableToken.value] = value;
            var nextToken = lexer.get_next_token();
            if (nextToken.type != TokenType.InKeyword || nextToken.value != "in")
            {
                Console.WriteLine($"Expected 'in' keyword after expression in 'let' expression at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
            var expressionValue = expression();
            return expressionValue;
        }
        else if (token.type == TokenType.Operator && token.value == "-")
        {
            var nextToken = lexer.get_next_token();
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
}