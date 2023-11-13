namespace mini_compiler;

/// <summary>
/// The Lexer class is responsible for scanning the input source code and returning a stream of tokens that can be used by the Parser to build an Abstract Syntax Tree (AST).
/// </summary>
public partial class Lexer
{
    private string text; // Contains the value of actual Token
    private int pos; // Contains the position of the actual Token
    private int line; // Contains the line of the actual Token
    private int column; // Contains the column of the actual Token
    private char current_char; // Contains the actual character
    private List<Token> readTokens = new List<Token>(); // Contains the tokens that have been read but not yet consumed
    public Lexer(string text) // Constructor
    {
        this.text = text;
        this.pos = 0;
        this.line = 1;
        this.column = 1;
        this.current_char = text[pos];
    }

    /// <summary>
    /// Recognizes and returns a token for a numeric value in the input stream.
    /// </summary>
    /// <returns>The token for the numeric value.</returns>
    private Token number()
    {
        string result = "";
        // Check the positive or negative sign
        if (current_char == '-')
        {
            result += current_char;
            advance();
        }
        // Recognize the whole part of the number
        while (current_char != '\0' && char.IsDigit(current_char))
        {
            result += current_char;
            advance();
        }
        // Recognize the decimal part of the number (if it exists)
        if (current_char == '.')
        {
            result += current_char;
            advance();

            while (current_char != '\0' && char.IsDigit(current_char))
            {
                result += current_char;
                advance();
            }
        }
        // Return the number token
        return new Token(TokenType.Number, result, line, column);
    }
    /// <summary>
    /// Scans and returns a token of type Identifier.
    /// </summary>
    /// <returns>The Identifier token.</returns>
    private Token identifier()
    {
        string result = "";
        while (current_char != '\0' && char.IsLetterOrDigit(current_char))
        {
            result += current_char;
            advance();
        }
        return new Token(TokenType.Identifier, result, line, column);
    }
    /// <summary>
    /// Consumes the current character and returns a separator token.
    /// </summary>
    /// <returns>A separator token.</returns>
    private Token separator()
    {
        advance();
        return new Token(TokenType.Separator, "@", line, column);
    }
    /// <summary>
    /// Parses a string literal token.
    /// </summary>
    /// <returns>The parsed string literal token.</returns>
    private Token string_literal()
    {
        string result = "";
        advance();
        while (current_char != '\0' && current_char != '"')
        {
            result += current_char;
            advance();
        }
        if (current_char == '\0')
        {
            Console.WriteLine($"Unterminated string literal at line {line} and column {column}");
        }
        advance(); // Consume the second '"' to advance to the next token
        return new Token(TokenType.StringLiteral, result, line, column);
    }
    /// <summary>
    /// Scans the input string for a keyword and returns a token representing the keyword.
    /// </summary>
    /// <returns>A token representing the keyword found in the input string.</returns>
    private Token keyword()
    {
        string result = "";
        while (current_char != '\0' && char.IsLetterOrDigit(current_char))
        {
            result += current_char;
            advance();
        }

        switch (result)
        {
            case "let":
                return new Token(TokenType.LetKeyword, result, line, column);
            case "function":
                return new Token(TokenType.FunctionKeyword, result, line, column);
            case "if":
                return new Token(TokenType.IfKeyword, result, line, column);
            case "else":
                return new Token(TokenType.ElseKeyword, result, line, column);
            case "in":
                return new Token(TokenType.InKeyword, result, line, column);
            case "print":
                return new Token(TokenType.PrintKeyword, result, line, column);
            default:
                return new Token(TokenType.Identifier, result, line, column);
        }
    }

    /// <summary>
    /// Function that trash one token
    /// </summary>
    /// <returns>None</returns>
    public void unget_token(Token token)
    {
        readTokens.Insert(0, token);
    }
    /// <summary>
    /// Function that advances one token when trying to parse and evaluate expressions
    /// </summary>
    /// <returns>Next token in the sourceCode</returns>
    public Token get_next_token()
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
                    skip_whitespace();
                    continue;
                case char c when char.IsDigit(c):
                    return number();
                case char c when char.IsLetter(c):
                    var token = keyword();
                    if (token.type == TokenType.FunctionKeyword)
                    {
                        return function_declaration();
                    }
                    else
                    {
                        return token;
                    }
                case '"':
                    return string_literal();
                case '+':
                    advance();
                    return new Token(TokenType.Operator, "+", line, column);
                case '-':
                    advance();
                    return new Token(TokenType.Operator, "-", line, column);
                case '*':
                    advance();
                    return new Token(TokenType.Operator, "*", line, column);
                case '/':
                    advance();
                    return new Token(TokenType.Operator, "/", line, column);
                case '(':
                    advance();
                    return new Token(TokenType.Punctuation, "(", line, column);
                case ')':
                    advance();
                    return new Token(TokenType.Punctuation, ")", line, column);
                case '{':
                    advance();
                    return new Token(TokenType.Punctuation, "{", line, column);
                case '}':
                    advance();
                    return new Token(TokenType.Punctuation, "}", line, column);
                case '[':
                    advance();
                    return new Token(TokenType.Punctuation, "[", line, column);
                case ']':
                    advance();
                    return new Token(TokenType.Punctuation, "]", line, column);
                case ';':
                    advance();
                    return new Token(TokenType.EOL, ";", line, column);
                case ':':
                    advance();
                    return new Token(TokenType.Punctuation, ":", line, column);
                case ',':
                    advance();
                    return new Token(TokenType.Punctuation, ",", line, column);
                case '=':
                    if (peek() == '>')
                    {
                        advance();
                        advance();
                        return new Token(TokenType.FLinq, "=>", line, column);
                    }
                    else if (peek() == '=')
                    {
                        advance();
                        advance();
                        return new Token(TokenType.ComparisonOperator, "==", line, column);
                    }
                    advance();
                    return new Token(TokenType.Operator, "=", line, column);
                case '<':
                    if (peek() == '=')
                    {
                        advance();
                        advance();
                        return new Token(TokenType.ComparisonOperator, "<=", line, column);
                    }
                    advance();
                    return new Token(TokenType.ComparisonOperator, "<", line, column);
                case '>':
                    if (peek() == '=')
                    {
                        advance();
                        advance();
                        return new Token(TokenType.ComparisonOperator, ">=", line, column);
                    }
                    advance();
                    return new Token(TokenType.ComparisonOperator, ">", line, column);
                case '!':
                    if (peek() == '=')
                    {
                        advance();
                        advance();
                        return new Token(TokenType.ComparisonOperator, "!=", line, column);
                    }
                    advance();
                    return new Token(TokenType.Operator, "!", line, column);
                case '%':
                    advance();
                    return new Token(TokenType.Operator, "%", line, column);
                case '^':
                    advance();
                    return new Token(TokenType.Operator, "^", line, column);
                case '.':
                    advance();
                    return new Token(TokenType.Operator, ".", line, column);
                case '@':
                    advance();
                    return new Token(TokenType.Operator, "@", line, column);
                case '\n':
                    advance();
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
}