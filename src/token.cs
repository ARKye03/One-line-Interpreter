//I wanna create a compiler a define some tokens first
using System;

namespace mini_compiler;
//Define some keywords, like function, if, else, etc

public enum TokenType
{
    Assignment,
    Number,
    StringLiteral,
    FunctionDeclaration,
    LetKeyword,
    IfKeyword,
    ElseKeyword,
    ThenKeyword,
    PrintKeyword,
    InKeyword,
    FunctionKeyword,
    InlineFunctionDeclaration,
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
    Div,
    Separator
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
public class AssignmentToken : Token
{
    public string VariableName { get; }
    public object Value { get; }

    public AssignmentToken(TokenType type, string variableName, object value, int line, int column)
        : base(type, $"{variableName} = {value}", line, column)
    {
        VariableName = variableName;
        Value = value;
    }
}



