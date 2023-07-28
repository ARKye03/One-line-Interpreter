//I wanna print the tokens and the expresions
using System;


namespace mini_compiler;
public class Expression{
    public static void Main(string[] args){
        while(true){
            Console.Write("> ");
            string? text = Console.ReadLine();
            if(text == "exit"){
                break;
            }
            Lexer lexer = new Lexer(text);
            Token token = lexer.get_next_token();
            while(token.type != TokenType.EOF){
                Console.WriteLine(token);
                token = lexer.get_next_token();
            }
        }
    }
}