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
                    if (nextToken.type == TokenType.Literal || nextToken.type == TokenType.Identifier)
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
            // Agregar más lógica para otros tipos de instrucciones si es necesario
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