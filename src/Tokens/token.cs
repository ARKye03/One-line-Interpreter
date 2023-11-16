namespace mini_compiler;

#region TokenTypeEnum
/// <summary>
/// Represents the type of a token in the mini_kompiler language.
/// </summary>
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
#endregion
#region TokenClass
/// <summary>
/// Represents a token in the source code.
/// </summary>
public class Token
{
    public TokenType type;
    public string value;
    public int line;
    public int column;

    /// <summary>
    /// Initializes a new instance of the Token class.
    /// </summary>
    /// <param name="type">The type of the token.</param>
    /// <param name="value">The value of the token.</param>
    /// <param name="line">The line number where the token was found.</param>
    /// <param name="column">The column number where the token was found.</param>
    public Token(TokenType type, string value, int line, int column)
    {
        this.type = type;
        this.value = value;
        this.line = line;
        this.column = column;
    }

    // I override the ToString method to be able to display the token in the console, I used it at startup to debug.
    public override string ToString()
    {
        return $"Token({type}, {value}, {line}, {column})";
    }
}
#endregion