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