//Here I wanna create a parser for my expresions
//Allow operations like sum, minus, etc betweem numbers and variables
using System;

namespace mini_compiler;

public class Parser{
    private Lexer lexer;
    private Token current_token;
    public Parser(Lexer lexer){
        this.lexer = lexer;
        this.current_token = lexer.get_next_token();
    }
    private void error(){
        throw new Exception("Invalid syntax");
    }
    private void eat(TokenType token_type){
        if(current_token.type == token_type){
            current_token = lexer.get_next_token();
        }else{
            error();
        }
    }
    private void factor(){
        Token token = current_token;
        if(token.type == TokenType.Literal){
            eat(TokenType.Literal);
        }else if(token.type == TokenType.Identifier){
            eat(TokenType.Identifier);
        }else if(token.type == TokenType.LParen){
            eat(TokenType.LParen);
            expr();
            eat(TokenType.RParen);
        }
    }
    private void term(){
        factor();
        while(current_token.type == TokenType.Mul || current_token.type == TokenType.Div){
            Token token = current_token;
            if(token.type == TokenType.Mul){
                eat(TokenType.Mul);
            }else if(token.type == TokenType.Div){
                eat(TokenType.Div);
            }
            factor();
        }
    }
    private void expr(){
        term();
        while(current_token.type == TokenType.Plus || current_token.type == TokenType.Minus){
            Token token = current_token;
            if(token.type == TokenType.Plus){
                eat(TokenType.Plus);
            }else if(token.type == TokenType.Minus){
                eat(TokenType.Minus);
            }
            term();
        }
    }
    public void parse(){
        expr();
    }
}
