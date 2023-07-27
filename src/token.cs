//I wanna create a compiler a define some tokens first
using System;

namespace mini_compiler;
//Define some keywords, like function, if, else, etc
//Define some types, like int, float, string, etc

public enum Keyword{
    Function,
    If,
    Else,
    For,
    While,
    Return,
    Break,
    Continue,
    True,
    False,
    Null
}
public enum Type{
    Int,
    Float,
    String,
    Bool,
    Void
}




public enum TokenType{

    Keyword,
    Type,
    Operator,
    Punctuation,
    Identifier,
    Literal,
    Comment,
    Error,
    EOL,
    EOF,
    LParen,
    RParen,
    LBrace,
    RBrace,
    LBracket,
    RBracket,
    Semicolon,
    Colon,
    Comma,
    Dot,
    Plus,
    Minus,
    Mul,
    Div
}

public class Token{
    public TokenType type;
    public string value;
    public int line;
    public int column;
    public Token(TokenType type, string value, int line, int column){
        this.type = type;
        this.value = value;
        this.line = line;
        this.column = column;
    }
    public override string ToString(){
        return $"Token({type}, {value}, {line}, {column})";
    }
}

