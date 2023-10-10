namespace mini_compiler;

public partial class Lexer
{
    private string text;
    private int pos;
    private int line;
    private int column;
    private char current_char;
    private List<Token> readTokens = new List<Token>();
    public Lexer(string text)
    {
        this.text = text;
        this.pos = 0;
        this.line = 1;
        this.column = 1;
        this.current_char = text[pos];
    }
    private Token number()
    {
        string result = "";
        // Verificar el signo positivo o negativo
        if (current_char == '-')
        {
            result += current_char;
            advance();
        }
        // Reconocer la parte entera del número
        while (current_char != '\0' && char.IsDigit(current_char))
        {
            result += current_char;
            advance();
        }
        // Reconocer la parte decimal del número (si existe)
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
        // Devolver el token de número
        return new Token(TokenType.Number, result, line, column);
    }
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
    private Token separator()
    {
        advance();
        return new Token(TokenType.Separator, "@", line, column);
    }
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
        advance(); // Consume el segundo '"' para avanzar al siguiente token
        return new Token(TokenType.StringLiteral, result, line, column);
    }
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
    /// <returns></returns>
    public void unget_token(Token token)
    {
        readTokens.Insert(0, token);
    }
    /// <summary>
    /// Function that advances one token when trying to parse and evaluate expressions
    /// </summary>
    /// <returns></returns>
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