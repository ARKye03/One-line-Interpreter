namespace mini_compiler;

public partial class Interpreter
{
    public Lexer lexer;
    public List<DFunction>? functions;

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
        //while ((token = lexer.get_next_token()).type != TokenType.EOF)
        //{
        token = lexer.get_next_token();
        if (token.type == TokenType.PrintKeyword)
        {
            // Analizar el contenido entre paréntesis y ejecutar la acción correspondiente
            var nextToken = lexer.get_next_token();
            if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
            {
                // Evaluar la expresión dentro del print y obtener el resultado
                var expressionValue = expression();

                // Verificar que se cierre el paréntesis
                nextToken = lexer.get_next_token();
                if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
                {
                    Console.WriteLine($"Expected ')' after value inside print statement at line {nextToken.line} and column {nextToken.column}");
                }
                else
                {
                    // Imprimir el resultado de la expresión evaluada
                    Console.WriteLine(expressionValue);
                }
            }
            else
            {
                Console.WriteLine($"Expected '(' after 'print' keyword at line {nextToken.line} and column {nextToken.column}");
            }
        }

        else if (token.type == TokenType.LetKeyword)
        {
            // Procesar la asignación de valores a variables
            assignment();
        }
        else if (token.type == TokenType.IfKeyword)
        {
            // Procesar la instrucción condicional
            Conditional();
        }

    }

    private Dictionary<string, object> variables = new Dictionary<string, object>();
    private void statement()
    {
        var token = lexer.get_next_token();

        if (token.type == TokenType.PrintKeyword)
        {
            // Analizar el contenido entre paréntesis y ejecutar la acción correspondiente
            var nextToken = lexer.get_next_token();
            if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
            {
                // Evaluar la expresión dentro de 'print(...)'
                var expressionValue = expression();

                // Verificar que se cierre el paréntesis
                nextToken = lexer.get_next_token();
                if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
                {
                    Console.WriteLine($"Expected ')' after value inside print statement at line {nextToken.line} and column {nextToken.column}");
                }

                Console.WriteLine(expressionValue); // Imprimir el resultado
            }
            else
            {
                Console.WriteLine($"Expected '(' after 'print' keyword at line {nextToken.line} and column {nextToken.column}");
            }
        }
        else if (token.type == TokenType.LetKeyword)
        {
            assignment();
        }
        else if (token.type == TokenType.IfKeyword)
        {
            Conditional();
        }
        // Agregar más lógica para otros tipos de instrucciones si es necesario
        else if (token.type == TokenType.EOF)
        {
            return; // Fin del programa
        }
        else
        {
            Console.WriteLine($"Invalid statement at line {token.line} and column {token.column}");
        }
        // Verificar si hay un punto y coma y avanzar al siguiente statement
        token = lexer.get_next_token(); // Reutilizar la variable 'token'
        if (token.type == TokenType.EOL)
        {
            statement(); // Siguiente statement
        }
        else
        {
            lexer.unget_token(token); // Devolver el token para que se analice en la siguiente iteración
        }
    }
}