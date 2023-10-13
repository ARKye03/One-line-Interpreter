namespace mini_compiler;
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
    private List<Token> SubstituteArgs(List<Token> tokens, Dictionary<string, object> argDict)
    {
        var substitutedTokens = new List<Token>();
        foreach (var token in tokens)
        {
            if (token.type == TokenType.Identifier && argDict.ContainsKey(token.value))
            {
                var argValue = argDict[token.value];
                substitutedTokens.Add(new Token(TokenType.Number, argValue.ToString()!, token.line, token.column));
            }
            else
            {
                substitutedTokens.Add(token);
            }
        }
        return substitutedTokens;
    }
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
}

public partial class Lexer
{
    private Token function_declaration()
    {
        // Al encontrar 'function', esperamos el nombre de la función seguido de paréntesis.
        advance(); // Avanzamos después de 'function'
        skip_whitespace();

        if (!char.IsLetter(current_char))
        {
            Console.WriteLine($"Expected a function name at line {line} and column {column}");
        }

        string function_name = "";
        while (current_char != '\0' && char.IsLetterOrDigit(current_char))
        {
            function_name += current_char;
            advance();
        }

        skip_whitespace();
        if (current_char != '(')
        {
            Console.WriteLine($"Expected '(' after function name at line {line} and column {column}");
        }

        // Analizar los parámetros de la función
        List<string> parameters = new List<string>();
        advance(); // Avanzar después del '('
        while (current_char != ')')
        {
            if (!char.IsLetter(current_char))
            {
                Console.WriteLine($"Expected a parameter name at line {line} and column {column}");
            }

            string parameter_name = "";
            while (current_char != '\0' && char.IsLetterOrDigit(current_char))
            {
                parameter_name += current_char;
                advance();
            }

            parameters.Add(parameter_name);
            skip_whitespace();

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
            Console.WriteLine($"Expected '=>' after function parameters at line {line} and column {column}");
        }

        // Analizar el cuerpo de la función como una expresión
        var expressionTokens = new List<Token>();
        nextToken = get_next_token();
        while (nextToken.type != TokenType.Semicolon && nextToken.type != TokenType.EOF)
        {
            expressionTokens.Add(nextToken);
            nextToken = get_next_token();
        }

        return new FunctionToken(TokenType.FunctionDeclaration, function_name, line, column, parameters, expressionTokens);
    }
}