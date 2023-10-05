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