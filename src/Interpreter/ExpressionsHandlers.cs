namespace mini_compiler;

public partial class Interpreter
{
    //Function to handle vars assignment in let-in expressions
    #region LetKeywords
    private void assignment()
    {
        var variableToken = lexer.get_next_token();
        if (variableToken.type != TokenType.Identifier)
        {
            Console.WriteLine($"Expected variable name after 'let' keyword at line {variableToken.line} and column {variableToken.column}");
            return;
        }
        var equalToken = lexer.get_next_token();
        if (equalToken.type != TokenType.Operator || equalToken.value != "=")
        {
            Console.WriteLine($"Expected '=' after variable name at line {equalToken.line} and column {equalToken.column}");
            return;
        }
        // Evaluar la expresión para obtener el valor asignado
        var value = expression();

        variables[variableToken.value] = value;

        var nextToken = lexer.get_next_token();
        if (nextToken.type == TokenType.Punctuation && nextToken.value == ",")
        {
            // Si hay una coma, continuar con la siguiente declaración
            assignment();
        }
        else if (nextToken.type != TokenType.InKeyword)
        {
            Console.WriteLine($"Expected 'in' or ',' after variable assignment at line {nextToken.line} and column {nextToken.column}");
            return;
        }

        // Ejecutar el siguiente statement 
        statement();
    }
    #endregion   
    //Function to conditional expressions
    #region ConditionalKeywords
    private void Conditional()
    {
        var token = lexer.get_next_token();
        if (token.type == TokenType.Punctuation && token.value == "(")
        {
            var leftValue = expression();
            var comparisonToken = lexer.get_next_token();
            if (comparisonToken.type != TokenType.ComparisonOperator)
            {
                Console.WriteLine($"Expected comparison operator after left-hand side of comparison in 'if' statement at line {comparisonToken.line} and column {comparisonToken.column}");
                return;
            }
            var rightValue = expression();
            var nextToken = lexer.get_next_token();
            if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
            {
                Console.WriteLine($"Expected ')' after right-hand side of comparison in 'if' statement at line {nextToken.line} and column {nextToken.column}");
                return;
            }
            var comparisonOperator = comparisonToken.value;
            bool comparisonResult;
            switch (comparisonOperator)
            {
                case "<":
                    comparisonResult = (float)leftValue < (float)rightValue;
                    break;
                case ">":
                    comparisonResult = (float)leftValue > (float)rightValue;
                    break;
                case "<=":
                    comparisonResult = (float)leftValue <= (float)rightValue;
                    break;
                case ">=":
                    comparisonResult = (float)leftValue >= (float)rightValue;
                    break;
                case "==":
                    comparisonResult = leftValue.Equals(rightValue);
                    break;
                case "!=":
                    comparisonResult = !leftValue.Equals(rightValue);
                    break;
                default:
                    Console.WriteLine($"Invalid comparison operator '{comparisonOperator}' in 'if' statement at line {comparisonToken.line} and column {comparisonToken.column}");
                    return;
            }
            if (comparisonResult)
            {
                statement();
            }
            else
            {
                // Saltar al else
                while (token.type != TokenType.ElseKeyword && token.type != TokenType.EOL)
                {
                    token = lexer.get_next_token();
                }
                if (token.type == TokenType.ElseKeyword)
                {
                    statement();
                }
                else
                {
                    Console.WriteLine($"Expected 'else' keyword at line {token.line} and column {token.column}");
                }
            }
        }
        else
        {
            Console.WriteLine($"Expected '(' after 'if' keyword at line {token.line} and column {token.column}");
        }
    }
    #endregion
    //Nothing
    #region FunctionKeywords
    #endregion
}