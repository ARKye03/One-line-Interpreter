namespace mini_compiler;

public partial class Lexer
{
    /// <summary>
    /// Recognizes and returns a token for a numeric value in the input stream.
    /// </summary>
    /// <returns>The token for the numeric value.</returns>
    private Token Number()
    {
        string result = "";
        // Check the positive or negative sign
        if (current_char == '-')
        {
            result += current_char;
            Advance();
        }
        // Recognize the whole part of the number
        while (current_char != '\0' && char.IsDigit(current_char))
        {
            result += current_char;
            Advance();
        }
        // Recognize the decimal part of the number (if it exists)
        if (current_char == '.')
        {
            result += current_char;
            Advance();

            while (current_char != '\0' && char.IsDigit(current_char))
            {
                result += current_char;
                Advance();
            }
        }
        // Return the number token
        return new Token(TokenType.Number, result, line, column);
    }
    /// <summary>
    /// Parses a string literal token.
    /// </summary>
    /// <returns>The parsed string literal token.</returns>
    private Token StringLiteral()
    {
        string result = "";
        Advance();
        while (current_char != '\0' && current_char != '"')
        {
            result += current_char;
            Advance();
        }
        if (current_char == '\0')
        {
            Console.WriteLine($"Unterminated string literal at line {line} and column {column}");
        }
        Advance(); // Consume the second '"' to advance to the next token
        return new Token(TokenType.StringLiteral, result, line, column);
    }
    /// <summary>
    /// Scans the input string for a keyword and returns a token representing the keyword.
    /// </summary>
    /// <returns>A token representing the keyword found in the input string.</returns>
    private Token Keyword()
    {
        string result = "";
        while (current_char != '\0' && char.IsLetterOrDigit(current_char))
        {
            result += current_char;
            Advance();
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

}