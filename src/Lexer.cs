using System;

namespace mini_compiler;

public class Lexer
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

    // This function advances the position of the lexer by one character and updates the current character accordingly.
    private void advance()
    {
        pos++;
        column++;
        if (pos > text.Length - 1)
        {
            current_char = '\0';
        }
        else
        {
            current_char = text[pos];
        }
    }

    // This function skips any whitespace characters in the input text by advancing the lexer until a non-whitespace character is found.
    private void skip_whitespace()
    {
        while (current_char != '\0' && char.IsWhiteSpace(current_char))
        {
            advance();
        }
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
    //private Token comment(){
    //    string result = "";
    //    advance();
    //    while(current_char != '\0' && current_char != '\n'){
    //        result += current_char;
    //        advance();
    //    }
    //    advance();
    //    return new Token(TokenType.Comment, result, line, column);
    //}
    private Token function_declaration()
    {
        // Al encontrar 'function', esperamos el nombre de la función seguido de paréntesis.
        advance(); // Avanzamos después de 'function'
        skip_whitespace();

        if (!char.IsLetter(current_char))
        {
            Console.WriteLine($"Expected a function name at line {line} and column {column}");
        }

        string function_name = "";
        while (current_char != '\0' && char.IsLetterOrDigit(current_char))
        {
            function_name += current_char;
            advance();
        }

        skip_whitespace();
        if (current_char != '(')
        {
            Console.WriteLine($"Expected '(' after function name at line {line} and column {column}");
        }

        // Analizar los parámetros de la función
        List<string> parameters = new List<string>();
        advance(); // Avanzar después del '('
        while (current_char != ')')
        {
            if (!char.IsLetter(current_char))
            {
                Console.WriteLine($"Expected a parameter name at line {line} and column {column}");
            }

            string parameter_name = "";
            while (current_char != '\0' && char.IsLetterOrDigit(current_char))
            {
                parameter_name += current_char;
                advance();
            }

            parameters.Add(parameter_name);
            skip_whitespace();

            if (current_char == ',')
            {
                advance();
                skip_whitespace();
            }
        }
        var nextToken = get_next_token();
        if (nextToken.type != TokenType.Operator || nextToken.value != "=>")
        {
            Console.WriteLine($"Expected '=>' after function parameters at line {line} and column {column}");
        }

        // Analizar el cuerpo de la función como una expresión
        var expressionTokens = new List<Token>();
        nextToken = get_next_token();
        while (nextToken.type != TokenType.Punctuation || nextToken.value != ";")
        {
            expressionTokens.Add(nextToken);
            nextToken = get_next_token();
        }

        advance(); // Avanzar después del ')'
        return new FunctionToken(TokenType.FunctionDeclaration, function_name, line, column, parameters);
    }
    public void unget_token(Token token)
    {
        readTokens.Insert(0, token);
    }
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
            if (char.IsWhiteSpace(current_char))
            {
                skip_whitespace();
                continue;
            }
            if (char.IsDigit(current_char))
            {
                return number();
            }
            if (char.IsLetter(current_char))
            {
                var token = keyword();
                if (token.type == TokenType.FunctionKeyword)
                {
                    // Reconocer el nombre de la función
                    var functionNameToken = get_next_token();
                    if (functionNameToken.type != TokenType.Identifier)
                    {
                        Console.WriteLine($"Expected a function name at line {line} and column {column}");
                    }

                    // Verificar y reconocer los paréntesis después del nombre de la función
                    var nextToken = get_next_token();
                    if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
                    {
                        var parameters = new List<string>();

                        // Reconocer los parámetros dentro de la declaración de función
                        nextToken = get_next_token();
                        while (nextToken.type == TokenType.Identifier)
                        {
                            parameters.Add(nextToken.value);
                            nextToken = get_next_token();
                            if (nextToken.type == TokenType.Punctuation && nextToken.value == ",")
                            {
                                nextToken = get_next_token();
                            }
                        }

                        if (nextToken.type == TokenType.Punctuation && nextToken.value == ")")
                        {
                            return new FunctionToken(TokenType.FunctionDeclaration, functionNameToken.value, functionNameToken.line, functionNameToken.column, parameters);
                        }
                        else
                        {
                            Console.WriteLine($"Expected ')' after function parameters at line {line} and column {column}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Expected '(' after function name at line {line} and column {column}");
                    }
                }
                else
                {
                    return token;
                }
            }

            if (current_char == '"')
            {
                return string_literal();
            }
            if (current_char == '+')
            {
                advance();
                return new Token(TokenType.Operator, "+", line, column);
            }
            if (current_char == '-')
            {
                advance();
                return new Token(TokenType.Operator, "-", line, column);
            }
            if (current_char == '*')
            {
                advance();
                return new Token(TokenType.Operator, "*", line, column);
            }
            if (current_char == '/')
            {
                advance();
                return new Token(TokenType.Operator, "/", line, column);
            }
            if (current_char == '(')
            {
                advance();
                return new Token(TokenType.Punctuation, "(", line, column);
            }
            if (current_char == ')')
            {
                advance();
                return new Token(TokenType.Punctuation, ")", line, column);
            }
            if (current_char == '{')
            {
                advance();
                return new Token(TokenType.Punctuation, "{", line, column);
            }
            if (current_char == '}')
            {
                advance();
                return new Token(TokenType.Punctuation, "}", line, column);
            }
            if (current_char == '[')
            {
                advance();
                return new Token(TokenType.Punctuation, "[", line, column);
            }
            if (current_char == ']')
            {
                advance();
                return new Token(TokenType.Punctuation, "]", line, column);
            }
            if (current_char == ';')
            {
                advance();
                return new Token(TokenType.EOL, ";", line, column);
            }
            if (current_char == ':')
            {
                advance();
                return new Token(TokenType.Punctuation, ":", line, column);
            }
            if (current_char == ',')
            {
                advance();
                return new Token(TokenType.Punctuation, ",", line, column);
            }
            if (current_char == '=')
            {
                advance();
                return new Token(TokenType.Operator, "=", line, column);
            }
            if (current_char == '<')
            {
                advance();
                return new Token(TokenType.Operator, "<", line, column);
            }
            if (current_char == '>')
            {
                advance();
                return new Token(TokenType.Operator, ">", line, column);
            }
            if (current_char == '!')
            {
                advance();
                return new Token(TokenType.Operator, "!", line, column);
            }
            if (current_char == '&')
            {
                advance();
                return new Token(TokenType.Operator, "&", line, column);
            }
            if (current_char == '|')
            {
                advance();
                return new Token(TokenType.Operator, "|", line, column);
            }
            if (current_char == '%')
            {
                advance();
                return new Token(TokenType.Operator, "%", line, column);
            }
            if (current_char == '^')
            {
                advance();
                return new Token(TokenType.Operator, "^", line, column);
            }
            if (current_char == '~')
            {
                advance();
                return new Token(TokenType.Operator, "~", line, column);
            }
            if (current_char == '.')
            {
                advance();
                return new Token(TokenType.Operator, ".", line, column);
            }
            if (current_char == '\n')
            {
                advance();
                line++;
                column = 1;
                continue;
            }
            Console.WriteLine($"Invalid character '{current_char}' at line {line} and column {column}");
        }
        return new Token(TokenType.EOF, null!, line, column);
    }
}