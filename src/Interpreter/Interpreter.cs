namespace mini_compiler;

public partial class Interpreter
{
    #region Variables
    public Lexer lexer; // Lexer, to lex while parsing
    private readonly Dictionary<string, object> variables = new(); // Variable assignment, this handles all vars
    #endregion
    #region InterpreterConstructor
    /// <summary>
    /// Constructor of the interpreter
    /// </summary>
    /// <param name="sourceCode">The source code to be interpreted</param>
    /// <returns> The interpreter object </returns>
    public Interpreter(string sourceCode)
    {
        lexer = new Lexer(sourceCode);
    }
    #endregion
    #region EvaluateSourceCode
    /// <summary>
    /// This is the equivalent to EvaluateExpression
    /// </summary>
    /// I learnt that up there by mistake LoL, so cool
    public void Run()
    {
        functions = DFunction.functions;
        Token token;
        token = lexer.GetNextToken();
        if (token.type == TokenType.PrintKeyword)
        {
            // Check the content between parenthesis and execute the corresponding action
            var nextToken = lexer.GetNextToken();
            if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
            {
                // Evaluate the expression inside the print and get the result
                var expressionValue = Expression();

                // Check that the parenthesis is closed
                nextToken = lexer.GetNextToken();
                if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
                {
                    // If the parenthesis is not closed, print an error
                    Console.WriteLine($"Expected ')' after value inside print statement at line {nextToken.line} and column {nextToken.column}");
                }
                else
                {
                    // Print the result of the evaluated expression
                    Console.WriteLine(expressionValue);
                }
            }
            else
            {
                // If the parenthesis is not opened, print an error
                Console.WriteLine($"Expected '(' after 'print' keyword at line {nextToken.line} and column {nextToken.column}");
            }
        }
        //If a "let" is found
        else if (token.type == TokenType.LetKeyword)
        {
            // Process the assignment of values to variables
            LetAssignment();
        }
        //If an If xd
        else if (token.type == TokenType.IfKeyword)
        {
            // Process the conditional instruction
            Conditional();
        }
    }
    #endregion
    #region StatementEvaluate
    /// <summary>
    /// This method analyzes and executes a statement based on the token type obtained from the lexer.
    /// </summary>
    // It's almost the most important here, it's the one that makes the magic happen
    // So this one is called everytime a statement is found, and makes possible to run expressions inside expressions
    private void Statement()
    {
        // Get the next token
        var token = lexer.GetNextToken();
        // Check the type of the token
        if (token.type == TokenType.PrintKeyword)
        {
            // Check the content between parenthesis and execute the corresponding action
            var nextToken = lexer.GetNextToken();
            if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
            {
                // Evaluate whats in => 'print(...)'
                var expressionValue = Expression();

                // Verify that the parenthesis is closed
                nextToken = lexer.GetNextToken();
                if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
                {
                    // If the parenthesis is not closed, print an error
                    Console.WriteLine($"Expected ')' after value inside print statement at line {nextToken.line} and column {nextToken.column}");
                }

                Console.WriteLine(expressionValue); // Print the result of the evaluated expression
            }
            else
            {
                // If the parenthesis is not opened, print an error
                Console.WriteLine($"Expected '(' after 'print' keyword at line {nextToken.line} and column {nextToken.column}");
            }
        }
        // If a "let" is found
        else if (token.type == TokenType.LetKeyword)
        {
            LetAssignment();
        }
        // If an If xd again
        else if (token.type == TokenType.IfKeyword)
        {
            Conditional();
        }
        else if (token.type == TokenType.Identifier || token.type == TokenType.Number)
        {
            //Do nothing
        }
        else if (token.type == TokenType.EOF)
        {
            return; // End of the show
        }
        else
        {
            // If the token is not recognized, print an error
            Console.WriteLine($"Invalid statement at line {token.line} and column {token.column}");
        }
        // Verify if there is a semicolon and advance to the next statement
        token = lexer.GetNextToken(); // Re-use the token 
        if (token.type == TokenType.EOL)
        {
            Statement(); // Next statement
        }
        else
        {
            lexer.UngetToken(token); // Return the token to be analyzed in the next iteration
        }
    }
    #endregion
}