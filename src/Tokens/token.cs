namespace mini_compiler;

public enum TokenType
{
    FLinq,
    ComparisonOperator,
    Number,
    StringLiteral,
    FunctionDeclaration,
    LetKeyword,
    IfKeyword,
    ElseKeyword,
    PrintKeyword,
    InKeyword,
    FunctionKeyword,
    Operator,
    Punctuation,
    Identifier,
    EOL,
    EOF,
    Semicolon,
    Separator
}
public class Token
{
    public TokenType type;
    public string value;
    public int line;
    public int column;
    public Token(TokenType type, string value, int line, int column)
    {
        this.type = type;
        this.value = value;
        this.line = line;
        this.column = column;
    }
    public override string ToString()
    {
        return $"Token({type}, {value}, {line}, {column})";
    }
}