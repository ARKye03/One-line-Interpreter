//I wanna create a compiler a define some tokens first
using System;

namespace mini_compiler;
//Define some keywords, like function, if, else, etc

public enum TokenType
{
    Assign,
    ArrowOperator,
    FunctionDeclaration,
    LetKeyword,
    IfKeyword,
    ElseKeyword,
    PrintKeyword,
    InKeyword,
    FunctionKeyword,
    Type,
    Operator,
    Punctuation,
    Identifier,
    Literal,
    Error,
    EOL,
    EOF,
    LParen,
    RParen,
    Semicolon,
    Colon,
    Comma,
    Dot,
    Plus,
    Minus,
    Mul,
    Div
}

public class Token
{
    public TokenType type;
    public string value;
    public int line;
    public int column;
    public Token(TokenType type, string value, int line, int column)
    {
        this.type = type;
        this.value = value;
        this.line = line;
        this.column = column;
    }
    public override string ToString()
    {
        return $"Token({type}, {value}, {line}, {column})";
    }
}

public class FunctionToken : Token
{
    public List<string> Parameters { get; }

    public FunctionToken(TokenType type, string value, int line, int column, List<string> parameters)
        : base(type, value, line, column)
    {
        Parameters = parameters;
    }

    public override string ToString()
    {
        return $"FunctionToken({type}, {value}, {line}, {column}, Parameters: [{string.Join(", ", Parameters)}])";
    }
}


