//parser
namespace mini_kompiler;

public class Parser
{
    public Parser(string text)
    {
        this.text = text;
    }
    public string text { get; set; }
    public int index { get; set; }
    public Token token { get; set; }
    public Token nextToken()
    {
        if (index >= text.Length)
        {
            return new Token(TokenType.EOF, "");
        }
        else
        {
            char c = text[index];
            index++;
            if (c == '+')
            {
                return new Token(TokenType.PLUS, "+");
            }
            else if (c == '-')
            {
                return new Token(TokenType.MINUS, "-");
            }
            else if (c == '*')
            {
                return new Token(TokenType.ASTERISK, "*");
            }
            else if (c == '/')
            {
                return new Token(TokenType.SLASH, "/");
            }
            else if (c == '(')
            {
                return new Token(TokenType.LPAREN, "(");
            }
            else if (c == ')')
            {
                return new Token(TokenType.RPAREN, ")");
            }
            else
            {
                return new Token(TokenType.ILLEGAL, "");
            }
        }
    }
    public void consume(TokenType type)
    {
        if (token.type == type)
        {
            token = nextToken();
        }
        else
        {
            throw new Exception("Unexpected token");
        }
    }
    public int expr()
    {
        int left = term();
        while (token.type == TokenType.PLUS || token.type == TokenType.MINUS)
        {
            TokenType op = token.type;
            consume(token.type);
            int right = term();
            if (op == TokenType.PLUS)
            {
                left += right;
            }
            else
            {
                left -= right;
            }
        }
        return left;
    }
    public int term()
    {
        int left = factor();
        while (token.type == TokenType.ASTERISK || token.type == TokenType.SLASH)
        {
            TokenType op = token.type;
            consume(token.type);
            int right = factor();
            if (op == TokenType.ASTERISK)
            {
                left *= right;
            }
            else
            {
                left /= right;
            }
        }
        return left;
    }
}
    
        