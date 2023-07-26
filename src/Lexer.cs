//Tokens:
namespace mini_kompiler;

public class Lexer
{
    public Tokenizer tokenizer;
    public Lexer()
    {
        this.tokenizer = new Tokenizer();
    }
    public void lexicalAnalysis(string text)
    {
        int line = 1;
        int column = 1;
        string value = "";
        string type = "";
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == ' ' || text[i] == '\n')
            {
                if (value != "")
                {
                    this.tokenizer.addToken(new Token(value, type, line, column));
                    value = "";
                    type = "";
                }
                if (text[i] == '\n')
                {
                    line++;
                    column = 1;
                }
            }
            else if (text[i] == '+' || text[i] == '-' || text[i] == '*' || text[i] == '/' || text[i] == '=' || text[i] == '>' || text[i] == '<')
            {
                if (value != "")
                {
                    this.tokenizer.addToken(new Token(value, type, line, column));
                    value = "";
                    type = "";
                }
                type = "Operador";
                value += text[i];
            }
            else if (text[i] == '!' || text[i] == '&' || text[i] == '|')
            {
                if (value != "")
                {
                    this.tokenizer.addToken(new Token(value, type, line, column));
                    value = "";
                    type = "";
                }
                type = "Condicion";
                value += text[i];
            }
            else if (text[i] == '(' || text[i] == ')' || text[i] == '{' || text[i] == '}')
            {
                if (value != "")
                {
                    this.tokenizer.addToken(new Token(value, type, line, column));
                    value = "";
                    type = "";
                }
                type = "Parentesis";
                value += text[i];
                this.tokenizer.addToken(new Token(value, type, line, column));
                value = "";
                type = "";
                if (text[i] == '\n')
                {
                    line++;
                    column = 1;
                }
                else
                {
                    column++;
                }
            }
            else if (text[i] == '"')
            {
                if (value != "")
                {
                    this.tokenizer.addToken(new Token(value, type, line, column));
                    value = "";
                    type = "";
                }
                type = "Cadena";
                value += text[i];
                i++;
                while (text[i] != '"')
                {
                    value += text[i];
                    i++;
                }
                this.tokenizer.addToken(new Token(value, type, line, column));
                value = "";
                type = "";
                if (text[i] == '\n')
                {
                    line++;
                    column = 1;
                }
                else
                {
                    column++;
                }
            }
            else
            {
                value += text[i];
                type = "Identificador";
                if (text[i] == '\n')
                {
                    line++;
                    column = 1;
                }
                else
                {
                    column++;
                }
            }
            if (i == text.Length - 1)
            {
                this.tokenizer.addToken(new Token(value, type, line, column));
            }
        }
        this.tokenizer.printTokens();
        Console.ReadLine();
        return;
    }
}
