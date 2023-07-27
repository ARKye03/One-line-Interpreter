//Here is my lexer for my expressions

using System;

namespace mini_compiler;

public class Lexer{
    private string text;
    private int pos;
    private int line;
    private int column;
    private char current_char;
    public Lexer(string text){
        this.text = text;
        this.pos = 0;
        this.line = 1;
        this.column = 1;
        this.current_char = text[pos];
    }
    private void advance(){
        pos++;
        column++;
        if(pos > text.Length - 1){
            current_char = '\0';
        }else{
            current_char = text[pos];
        }
    }
    private void skip_whitespace(){
        while(current_char != '\0' && char.IsWhiteSpace(current_char)){
            advance();
        }
    }
    private Token number(){
        string result = "";
        while(current_char != '\0' && char.IsDigit(current_char)){
            result += current_char;
            advance();
        }
        if(current_char == '.'){
            result += current_char;
            advance();
            while(current_char != '\0' && char.IsDigit(current_char)){
                result += current_char;
                advance();
            }
            return new Token(TokenType.Literal, result, line, column);
        }
        return new Token(TokenType.Literal, result, line, column);
    }
    private Token identifier(){
        string result = "";
        while(current_char != '\0' && char.IsLetterOrDigit(current_char)){
            result += current_char;
            advance();
        }
        return new Token(TokenType.Identifier, result, line, column);
    }
    private Token string_literal(){
        string result = "";
        advance();
        while(current_char != '\0' && current_char != '"'){
            result += current_char;
            advance();
        }
        advance();
        return new Token(TokenType.Literal, result, line, column);
    }
    private Token comment(){
        string result = "";
        advance();
        while(current_char != '\0' && current_char != '\n'){
            result += current_char;
            advance();
        }
        advance();
        return new Token(TokenType.Comment, result, line, column);
    }
    public Token get_next_token(){
        while(current_char != '\0'){
            if(char.IsWhiteSpace(current_char)){
                skip_whitespace();
                continue;
            }
            if(char.IsDigit(current_char)){
                return number();
            }
            if(char.IsLetter(current_char)){
                return identifier();
            }
            if(current_char == '"'){
                return string_literal();
            }
            if(current_char == '/'){
                return comment();
            }
            if(current_char == '+'){
                advance();
                return new Token(TokenType.Operator, "+", line, column);
            }
            if(current_char == '-'){
                advance();
                return new Token(TokenType.Operator, "-", line, column);
            }
            if(current_char == '*'){
                advance();
                return new Token(TokenType.Operator, "*", line, column);
            }
            if(current_char == '/'){
                advance();
                return new Token(TokenType.Operator, "/", line, column);
            }
            if(current_char == '('){
                advance();
                return new Token(TokenType.Punctuation, "(", line, column);
            }
            if(current_char == ')'){
                advance();
                return new Token(TokenType.Punctuation, ")", line, column);
            }
            if(current_char == '{'){
                advance();
                return new Token(TokenType.Punctuation, "{", line, column);
            }
            if(current_char == '}'){
                advance();
                return new Token(TokenType.Punctuation, "}", line, column);
            }
            if(current_char == '['){
                advance();
                return new Token(TokenType.Punctuation, "[", line, column);
            }
            if(current_char == ']'){
                advance();
                return new Token(TokenType.Punctuation, "]", line, column);
            }
            if(current_char == ';'){
                advance();
                return new Token(TokenType.Punctuation, ";", line, column);
            }
            if(current_char == ':'){
                advance();
                return new Token(TokenType.Punctuation, ":", line, column);
            }
            if(current_char == ','){
                advance();
                return new Token(TokenType.Punctuation, ",", line, column);
            }
            if(current_char == '='){
                advance();
                return new Token(TokenType.Operator, "=", line, column);
            }
            if(current_char == '<'){
                advance();
                return new Token(TokenType.Operator, "<", line, column);
            }
            if(current_char == '>'){
                advance();
                return new Token(TokenType.Operator, ">", line, column);
            }
            if(current_char == '!'){
                advance();
                return new Token(TokenType.Operator, "!", line, column);
            }
            if(current_char == '&'){
                advance();
                return new Token(TokenType.Operator, "&", line, column);
            }
            if(current_char == '|'){
                advance();
                return new Token(TokenType.Operator, "|", line, column);
            }
            if(current_char == '%'){
                advance();
                return new Token(TokenType.Operator, "%", line, column);
            }
            if(current_char == '^'){
                advance();
                return new Token(TokenType.Operator, "^", line, column);
            }
            if(current_char == '~'){
                advance();
                return new Token(TokenType.Operator, "~", line, column);
            }
            if(current_char == '.'){
                advance();
                return new Token(TokenType.Operator, ".", line, column);
            }
            if(current_char == '\n'){
                advance();
                line++;
                column = 1;
                continue;
            }
            throw new Exception($"Invalid character '{current_char}' at line {line} and column {column}");
        }
        return new Token(TokenType.EOF, null, line, column);
    }    
}