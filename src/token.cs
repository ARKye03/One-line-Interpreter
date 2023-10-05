namespace mini_compiler;

public enum TokenType
{
    FLinq,
    ComparisonOperator,
    Assignment,
    Number,
    StringLiteral,
    FunctionDeclaration,
    LetKeyword,
    IfKeyword,
    ElseKeyword,
    ThenKeyword,
    PrintKeyword,
    InKeyword,
    FunctionKeyword,
    InlineFunctionDeclaration,
    Type,
    Operator,
    Punctuation,
    Identifier,
    Literal,
    Error,
    EOL,
    EOF,
    LParen,
    RParen,
    Semicolon,
    Colon,
    Comma,
    Dot,
    Plus,
    Minus,
    Mul,
    Div,
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