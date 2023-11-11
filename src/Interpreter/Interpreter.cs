using System.Numerics;

namespace mini_compiler;

public partial class Interpreter
{
    public Lexer lexer; // Lexer, to lex while parsing
    public List<DFunction>? functions; // List of declarable functions, to be used in the program

    /// <summary>
    /// Represents a list of built-in mathematical functions used by the interpreter.
    /// </summary>
    public List<Functions> functions2 = new List<Functions>
    {
        new Functions("Sin", 1, args => Math.Sin(Convert.ToSingle(args[0]))),
        new Functions("Cos", 1, args => Math.Cos(Convert.ToSingle(args[0]))),
        new Functions("Tan", 1, args => Math.Tan(Convert.ToSingle(args[0]))),
        new Functions("Log", 1, args => Math.Log(Convert.ToSingle(args[0]))),
        new Functions("Ln", 1, args => Math.Log(Convert.ToSingle(args[0]), Math.E)),
        new Functions("Sqrt", 1, args => Math.Sqrt(Convert.ToSingle(args[0]))),
        new Functions("Abs", 1, args => Math.Abs(Convert.ToSingle(args[0]))),
        new Functions("Pow", 2, args => Math.Pow(Convert.ToSingle(args[0]), Convert.ToSingle(args[1]))),
        new Functions("Exp", 1, args => Math.Exp(Convert.ToSingle(args[0]))),
        new Functions("Floor", 1, args => Math.Floor(Convert.ToSingle(args[0]))),
        new Functions("Ceil", 1, args => Math.Ceiling(Convert.ToSingle(args[0]))),
        new Functions("Round", 1, args => Math.Round(Convert.ToSingle(args[0]))),
        new Functions("Rand", 2, args =>
        {
            int min = Convert.ToInt32(args[0]);
            int max = Convert.ToInt32(args[1]);
            return new Random().Next(min, max);
        })
    };




    /// <summary>
    /// Constructor of the interpreter
    /// </summary>
    /// <param name="sourceCode">The source code to be interpreted</param>
    /// <returns> The interpreter object </returns>
    public Interpreter(string sourceCode)
    {
        lexer = new Lexer(sourceCode);
    }
    /// <summary>
    /// This is the equivalent to EvaluateExpression
    /// </summary>
    /// I learnt that up there by mistake LoL, so cool
    public void Run()
    {
        functions = FunctionToken.functions;
        Token token;
        //while ((token = lexer.get_next_token()).type != TokenType.EOF) //Here to multiline idea 
        //{
        token = lexer.get_next_token();
        if (token.type == TokenType.PrintKeyword)
        {
            // Check the content between parenthesis and execute the corresponding action
            var nextToken = lexer.get_next_token();
            if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
            {
                // Evaluate the expression inside the print and get the result
                var expressionValue = expression();

                // Check that the parenthesis is closed
                nextToken = lexer.get_next_token();
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
            assignment();
        }
        //If an If xd
        else if (token.type == TokenType.IfKeyword)
        {
            // Process the conditional instruction
            Conditional();
        }

    }
    // Variable assignment, this handles all vars
    private Dictionary<string, object> variables = new Dictionary<string, object>();

    /// <summary>
    /// This method analyzes and executes a statement based on the token type obtained from the lexer.
    /// </summary>
    // It's almost the most important here, it's the one that makes the magic happen
    // So this one is called everytime a statement is found, and makes possible to run expressions inside expressions
    private void statement()
    {
        // Get the next token
        var token = lexer.get_next_token();
        // Check the type of the token
        if (token.type == TokenType.PrintKeyword)
        {
            // Check the content between parenthesis and execute the corresponding action
            var nextToken = lexer.get_next_token();
            if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
            {
                // Evaluate whats in => 'print(...)'
                var expressionValue = expression();

                // Verify that the parenthesis is closed
                nextToken = lexer.get_next_token();
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
            assignment();
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
        token = lexer.get_next_token(); // Re-use the token 
        if (token.type == TokenType.EOL)
        {
            statement(); // Next statement
        }
        else
        {
            lexer.unget_token(token); // Return the token to be analyzed in the next iteration
        }
    }
}