//Tokens:
//- Identificadores
//- Numeros
//- Operadores
//- Condiciones
namespace mini_kompiler;
public enum TokenType
{
    ILLEGAL,
    EOF,
    //Identifiers + Literals
    IDENT,
    INT,
    //Operators
    ASSIGN,
    PLUS,
    MINUS,
    ASTERISK,
    SLASH,
    LT,
    GT,
    EQ,
    NOT_EQ,
    //Delimiters
    COMMA,
    SEMICOLON,
    LPAREN,
    RPAREN,
    LBRACE,
    RBRACE,
    //Keywords
    FUNCTION,
    LET,
    TRUE,
    FALSE,
    IF,
    ELSE,
    RETURN
}

public class Token
    {
        public string value;
        public TokenType type;
        public int line;
        public int column;
        public Token(string value, TokenType type, int line, int column)
        {
            this.value = value;
            this.type = type;
            this.line = line;
            this.column = column;
        }
    }
    public class Tokenizer
    {
        private string _source;
        private int _pos;
        private int _line;
        private int _column;
        public Tokenizer(string source)
        {
            _source = source;
            _pos = 0;
            _line = 1;
            _column = 1;
        }
        public Token NextToken()
        {
            while (char.IsWhiteSpace(_source[_pos]))
            {
                if (_source[_pos] == '\n')
                {
                    _line++;
                    _column = 1;
                }
                _pos++;
            }
            if (_pos >= _source.Length)
            {
                return new Token("", TokenType.EOF, _line, _column);
            }
            char current = _source[_pos];
            switch (current)
            {
                case '(':
                    _pos++;
                    _column++;
                    return new Token("(", TokenType.LPAREN, _line, _column);
                case ')':
                    _pos++;
                    _column++;
                    return new Token(")", TokenType.RPAREN, _line, _column);
                case '{':
                    _pos++;
                    _column++;
                    return new Token("{", TokenType.LBRACE, _line, _column);
                case '}':
                    _pos++;
                    _column++;
                    return new Token("}", TokenType.RBRACE, _line, _column);
                case ',':
                    _pos++;
                    _column++;
                    return new Token(",", TokenType.COMMA, _line, _column);
                case ';':
                    _pos++;
                    _column++;
                    return new Token(";", TokenType.SEMICOLON, _line, _column);
                case '+':
                    _pos++;
                    _column++;
                    return new Token("+", TokenType.PLUS, _line, _column);
                case '-':
                    _pos++;
                    _column++;
                    return new Token("-", TokenType.MINUS, _line, _column);
                case '*':
                    _pos++;
                    _column++;
                    return new Token("*", TokenType.ASTERISK, _line, _column);
                case '/':
                    _pos++;
                    _column++;
                    return new Token("/", TokenType.SLASH, _line, _column);
                case '<':
                    _pos++;
                    _column++;
                    return new Token("<", TokenType.LT, _line, _column);
                case '>':
                    _pos++;
                    _column++;
                    return new Token(">", TokenType.GT, _line, _column);
                case '=':
                    _pos++;
                    _column++;
                    return new Token("=", TokenType.EQ, _line, _column);
                case '!':
                    _pos++;
                    _column++;
                    return new Token("!", TokenType.NOT_EQ, _line, _column);
                case ' ':
                    _pos++;
                    _column++;
                    return NextToken();
            }
            if (char.IsLetter(current))
            {
                string value = "";
                while (char.IsLetterOrDigit(current) || current == '_')
                {
                    value += current;
                    _pos++;
                    _column++;
                    if (_pos >= _source.Length)
                    {
                        break;
                    }
                    current = _source[_pos];
                }
                if (value == "let")
                {
                    return new Token(value, TokenType.LET, _line, _column);
                }
                if (value == "function")
                {
                    return new Token(value, TokenType.FUNCTION, _line, _column);
                }
                if (value == "if")
                {
                    return new Token(value, TokenType.IF, _line, _column);
                }
                if (value == "else")
                {
                    return new Token(value, TokenType.ELSE, _line, _column);
                }
                if (value == "return")
                {
                    return new Token(value, TokenType.RETURN, _line, _column);
                }
                if (value == "true")
                {
                    return new Token(value, TokenType.TRUE, _line, _column);
                }
                if (value == "false")
                {
                    return new Token(value, TokenType.FALSE, _line, _column);
                }
                return new Token(value, TokenType.IDENT, _line, _column);
            }
            if (char.IsDigit(current))
            {
                string value = "";
                while (char.IsDigit(current))
                {
                    value += current;
                    _pos++;
                    _column++;
                    if (_pos >= _source.Length)
                    {
                        break;
                    }
                    current = _source[_pos];
                }
                return new Token(value, TokenType.INT, _line, _column);
            }
            _pos++;
            _column++;
            return NextToken();
        }
        public Token PeekToken()
        {
            int pos = _pos;
            int line = _line;
            int column = _column;
            Token token = NextToken();
            _pos = pos;
            _line = line;
            _column = column;
            return token;
        }
        public Token PeekToken(int offset)
        {
            int pos = _pos;
            int line = _line;
            int column = _column;
            for (int i = 0; i < offset; i++)
            {
                NextToken();
            }
            Token token = NextToken();
            _pos = pos;
            _line = line;
            _column = column;
            return token;
        }
        public Token PeekToken(TokenType type)
        {
            int pos = _pos;
            int line = _line;
            int column = _column;
            Token token = NextToken();
            if (token.type != type)
            {
                _pos = pos;
                _line = line;
                _column = column;
                return token;
            }
            _pos = pos;
            _line = line;
            _column = column;
            return token;
        }
    }