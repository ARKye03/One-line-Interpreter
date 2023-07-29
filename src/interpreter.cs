//I wanna print the tokens and the expresions
using System;
namespace mini_compiler;



public class Interpreter
{
    public Lexer lexer;

    public Interpreter(string sourceCode)
    {
        lexer = new Lexer(sourceCode);
    }
    public void Run()
    {
        Token token;
        while ((token = lexer.get_next_token()).type != TokenType.EOF)
        {
            if (token.type == TokenType.PrintKeyword)
            {
                // Analizar el contenido entre paréntesis y ejecutar la acción correspondiente
                var nextToken = lexer.get_next_token();
                if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
                {
                    nextToken = lexer.get_next_token();
                    if (nextToken.type == TokenType.Literal || nextToken.type == TokenType.Number || nextToken.type == TokenType.StringLiteral)
                    {
                        // Ejecutar la acción de imprimir el valor
                        Console.WriteLine(nextToken.value);
                    }
                    else
                    {
                        throw new Exception($"Invalid value inside print statement at line {nextToken.line} and column {nextToken.column}");
                    }

                    // Verificar que se cierre el paréntesis
                    nextToken = lexer.get_next_token();
                    if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
                    {
                        throw new Exception($"Expected ')' after value inside print statement at line {nextToken.line} and column {nextToken.column}");
                    }
                }
                else
                {
                    throw new Exception($"Expected '(' after 'print' keyword at line {nextToken.line} and column {nextToken.column}");
                }
            }
            else if (token.type == TokenType.LetKeyword)
            {
                // Procesar la asignación de valores a variables
                assignment();
            }
            // Agregar más lógica para otros tipos de instrucciones si es necesario
        }
    }

    private Dictionary<string, object> variables = new Dictionary<string, object>();

    private void assignment()
    {
        var variableToken = lexer.get_next_token();
        if (variableToken.type != TokenType.Identifier)
        {
            throw new Exception($"Expected variable name after 'let' keyword at line {variableToken.line} and column {variableToken.column}");
        }

        var equalToken = lexer.get_next_token();
        if (equalToken.type != TokenType.Operator || equalToken.value != "=")
        {
            throw new Exception($"Expected '=' after variable name at line {equalToken.line} and column {equalToken.column}");
        }

        // Evaluar la expresión para obtener el valor asignado
        var value = expression();

        variables[variableToken.value] = value;

        var inToken = lexer.get_next_token();
        if (inToken.type != TokenType.InKeyword)
        {
            throw new Exception($"Expected 'in' after variable assignment at line {inToken.line} and column {inToken.column}");
        }

        // Ejecutar el siguiente statement (en este caso, solo se permite print)
        statement();
    }
    private void statement()
    {
        var token = lexer.get_next_token();
        if (token.type == TokenType.PrintKeyword)
        {
            var nextToken = lexer.get_next_token();
            if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
            {
                // Evaluar la expresión dentro del paréntesis
                var expressionValue = expression();
                Console.WriteLine(expressionValue);

                // Verificar que se cierre el paréntesis
                var closingParenToken = lexer.get_next_token();
                if (closingParenToken.type != TokenType.Punctuation || closingParenToken.value != ")")
                {
                    throw new Exception($"Expected ')' after value inside print statement at line {closingParenToken.line} and column {closingParenToken.column}");
                }
            }
            else
            {
                throw new Exception($"Expected '(' after 'print' keyword at line {nextToken.line} and column {nextToken.column}");
            }
        }
        else if (token.type == TokenType.LetKeyword)
        {
            assignment();
        }
        // Agregar más lógica para otros tipos de instrucciones si es necesario
    }
    private object expression()
    {
        var token = lexer.get_next_token();

        if (token.type == TokenType.Number || token.type == TokenType.StringLiteral)
        {
            return token.value;
        }
        else if (token.type == TokenType.Identifier)
        {
            var variableName = token.value;
            if (variables.ContainsKey(variableName))
            {
                return variables[variableName];
            }
            throw new Exception($"Variable '{variableName}' not found at line {token.line} and column {token.column}");
        }
        else
        {
            throw new Exception($"Invalid expression at line {token.line} and column {token.column}");
        }
    }

}
class c_ui
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");
            string? sourceCode = Console.ReadLine();
            if (sourceCode == "exit")
            {
                break;
            }
            Interpreter interpreter = new Interpreter(sourceCode);
            interpreter.Run();
        }
    }
}


//public class Expression{
//    public static void Main(string[] args){
//        while(true){
//            Console.Write("> ");
//            string? text = Console.ReadLine();
//            if(text == "exit"){
//                break;
//            }
//            Lexer lexer = new Lexer(text);
//            Token token = lexer.get_next_token();
//            while(token.type != TokenType.EOF){
//                Console.WriteLine(token);
//                token = lexer.get_next_token();
//            }
//        }
//    }
//}