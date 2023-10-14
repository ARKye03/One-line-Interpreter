namespace mini_compiler;

// Token types
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
// Clase Token
// Tiene agregado una propiedad 'column' para saber en que columna esta el token
// Tiene agregado una propiedad 'line' para saber en que linea esta el token => Pensando en el futuro, para posibilitar desarrollo multilinea
// Tiene agregado una propiedad 'value' para saber el valor del token
// Tiene agregado una propiedad 'type' para saber el tipo del token, enum de arriba
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
    // Sobreescribo el metodo ToString para poder mostrar el token en la consola, lo use al inicio para debuggear.
    public override string ToString()
    {
        return $"Token({type}, {value}, {line}, {column})";
    }
}