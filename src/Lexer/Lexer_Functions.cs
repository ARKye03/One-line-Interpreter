namespace mini_compiler;
public partial class Lexer
{
    /// <summary>
    /// Advances the position of the lexer to the next character in the input text.
    /// </summary>
    private void Advance()
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
    /// <summary>
    /// Skips all whitespace characters until a non-whitespace character is found.
    /// </summary>
    private void SkipWhitespace()
    {
        while (current_char != '\0' && char.IsWhiteSpace(current_char))
        {
            Advance();
        }
    }
    /// <summary>
    /// Returns the next character in the input string without consuming it.
    /// </summary>
    /// <returns>The next character in the input string, or '\0' if there are no more characters.</returns>
    private char Peek()
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
    /// <summary>
    /// Function that trash one token
    /// </summary>
    /// <returns>None</returns>
    public void UngetToken(Token token)
    {
        readTokens.Insert(0, token);
    }

}