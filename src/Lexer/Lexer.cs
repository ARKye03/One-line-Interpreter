namespace mini_compiler;

/// <summary>
/// The Lexer class is responsible for scanning the input source code and returning a stream of tokens that can be used by the Parser to build an Abstract Syntax Tree (AST).
/// </summary>
public partial class Lexer
{
    #region SomeVars
    private readonly string text; // Contains the value of actual Token
    private int pos; // Contains the position of the actual Token
    private int line; // Contains the line of the actual Token
    private int column; // Contains the column of the actual Token
    private char current_char; // Contains the actual character
    private readonly List<Token> readTokens = new(); // Contains the tokens that have been read but not yet consumed
    #endregion
    #region LexerConstructor
    public Lexer(string text) // Constructor
    {
        this.text = text;
        this.pos = 0;
        this.line = 1;
        this.column = 1;
        this.current_char = text[pos];
    }
    #endregion
    #region MainLexerFunction
    /// <summary>
    /// Function that advances one token when trying to parse and evaluate expressions
    /// </summary>
    /// <returns>Next token in the sourceCode</returns>
    public Token GetNextToken()
    {
        if (readTokens.Count > 0)
        {
            Token token = readTokens[0];
            readTokens.RemoveAt(0);
            return token;
        }
        while (current_char != '\0')
        {
            switch (current_char)
            {
                case char c when char.IsWhiteSpace(c):
                    SkipWhitespace();
                    continue;
                case char c when char.IsDigit(c):
                    return Number();
                case char c when char.IsLetter(c):
                    var token = Keyword();
                    if (token.type == TokenType.FunctionKeyword)
                    {
                        return FunctionDeclaration();
                    }
                    else
                    {
                        return token;
                    }
                case '"':
                    return StringLiteral();
                case '+':
                    Advance();
                    return new Token(TokenType.Operator, "+", line, column);
                case '-':
                    Advance();
                    return new Token(TokenType.Operator, "-", line, column);
                case '*':
                    Advance();
                    return new Token(TokenType.Operator, "*", line, column);
                case '/':
                    Advance();
                    return new Token(TokenType.Operator, "/", line, column);
                case '(':
                    Advance();
                    return new Token(TokenType.Punctuation, "(", line, column);
                case ')':
                    Advance();
                    return new Token(TokenType.Punctuation, ")", line, column);
                case '{':
                    Advance();
                    return new Token(TokenType.Punctuation, "{", line, column);
                case '}':
                    Advance();
                    return new Token(TokenType.Punctuation, "}", line, column);
                case '[':
                    Advance();
                    return new Token(TokenType.Punctuation, "[", line, column);
                case ']':
                    Advance();
                    return new Token(TokenType.Punctuation, "]", line, column);
                case ';':
                    Advance();
                    return new Token(TokenType.EOL, ";", line, column);
                case ':':
                    Advance();
                    return new Token(TokenType.Punctuation, ":", line, column);
                case ',':
                    Advance();
                    return new Token(TokenType.Punctuation, ",", line, column);
                case '=':
                    if (Peek() == '>')
                    {
                        Advance();
                        Advance();
                        return new Token(TokenType.FLinq, "=>", line, column);
                    }
                    else if (Peek() == '=')
                    {
                        Advance();
                        Advance();
                        return new Token(TokenType.ComparisonOperator, "==", line, column);
                    }
                    Advance();
                    return new Token(TokenType.Operator, "=", line, column);
                case '<':
                    if (Peek() == '=')
                    {
                        Advance();
                        Advance();
                        return new Token(TokenType.ComparisonOperator, "<=", line, column);
                    }
                    Advance();
                    return new Token(TokenType.ComparisonOperator, "<", line, column);
                case '>':
                    if (Peek() == '=')
                    {
                        Advance();
                        Advance();
                        return new Token(TokenType.ComparisonOperator, ">=", line, column);
                    }
                    Advance();
                    return new Token(TokenType.ComparisonOperator, ">", line, column);
                case '!':
                    if (Peek() == '=')
                    {
                        Advance();
                        Advance();
                        return new Token(TokenType.ComparisonOperator, "!=", line, column);
                    }
                    Advance();
                    return new Token(TokenType.Operator, "!", line, column);
                case '%':
                    Advance();
                    return new Token(TokenType.Operator, "%", line, column);
                case '^':
                    Advance();
                    return new Token(TokenType.Operator, "^", line, column);
                case '.':
                    Advance();
                    return new Token(TokenType.Operator, ".", line, column);
                case '@':
                    Advance();
                    return new Token(TokenType.Operator, "@", line, column);
                case '\n':
                    Advance();
                    line++;
                    column = 1;
                    continue;
                default:
                    Console.WriteLine($"Invalid character '{current_char}' at line {line} and column {column}");
                    break;
            }
        }
        return new Token(TokenType.EOF, null!, line, column);
    }
    #endregion
}