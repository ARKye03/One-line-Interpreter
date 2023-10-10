namespace mini_compiler;
public partial class Lexer
{
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
    // This function take a peek to the next token without getting it
    private char peek()
    {
        var peek_pos = pos + 1;
        if (peek_pos >= text.Length)
        {
            return '\0';
        }
        else
        {
            return text[peek_pos];
        }
    }

}