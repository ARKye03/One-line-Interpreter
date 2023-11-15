namespace mini_compiler;

public partial class Interpreter
{
    //Function to handle vars assignment in let-in expressions
    #region LetKeywords
    private void assignment()
    {
        var variableToken = lexer.GetNextToken();
        if (variableToken.type != TokenType.Identifier)
        {
            Console.WriteLine($"Expected variable name after 'let' keyword at line {variableToken.line} and column {variableToken.column}");
            return;
        }
        var equalToken = lexer.GetNextToken();
        if (equalToken.type != TokenType.Operator || equalToken.value != "=")
        {
            Console.WriteLine($"Expected '=' after variable name at line {equalToken.line} and column {equalToken.column}");
            return;
        }
        // Evaluate the expression to obtain the assigned value
        var value = Expression();

        variables[variableToken.value] = value;

        var nextToken = lexer.GetNextToken();
        if (nextToken.type == TokenType.Punctuation && nextToken.value == ",")
        {
            // If there is a comma, continue with the next statement
            assignment();
        }
        else if (nextToken.type != TokenType.InKeyword)
        {
            Console.WriteLine($"Expected 'in' or ',' after variable assignment at line {nextToken.line} and column {nextToken.column}");
            return;
        }

        // Execute the following statement
        statement();
    }
    #endregion   
    //Function to conditional expressions
    #region ConditionalKeywords
    private void Conditional()
    {
        var token = lexer.GetNextToken();
        if (token.type == TokenType.Punctuation && token.value == "(")
        {
            var leftValue = Expression();
            var comparisonToken = lexer.GetNextToken();
            if (comparisonToken.type != TokenType.ComparisonOperator)
            {
                Console.WriteLine($"Expected comparison operator after left-hand side of comparison in 'if' statement at line {comparisonToken.line} and column {comparisonToken.column}");
                return;
            }
            var rightValue = Expression();
            var nextToken = lexer.GetNextToken();
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
                // Jump to else
                while (token.type != TokenType.ElseKeyword && token.type != TokenType.EOL)
                {
                    token = lexer.GetNextToken();
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
    #region ConditionalOverload
    private object RConditional()
    {
        var token = lexer.GetNextToken();
        if (token.type == TokenType.Punctuation && token.value == "(")
        {
            var leftValue = Expression();
            var comparisonToken = lexer.GetNextToken();
            if (comparisonToken.type != TokenType.ComparisonOperator)
            {
                Console.WriteLine($"Expected comparison operator after left-hand side of comparison in 'if' statement at line {comparisonToken.line} and column {comparisonToken.column}");
                return null!;
            }
            var rightValue = Expression();
            var nextToken = lexer.GetNextToken();
            if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
            {
                Console.WriteLine($"Expected ')' after right-hand side of comparison in 'if' statement at line {nextToken.line} and column {nextToken.column}");
                return null!;
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
                    return null!;
            }
            if (comparisonResult)
            {
                var result = Expression();
                // Skip the else block
                while (true)
                {
                    token = lexer.GetNextToken();
                    if (token.type == TokenType.InKeyword)
                    {
                        lexer.UngetToken(token); // Put the InKeyword back into the lexer
                        break;
                    }
                    else if (token.type == TokenType.EOL)
                    {
                        break;
                    }
                }
                return result;
            }
            {
                // Jump to else
                while (token.type != TokenType.ElseKeyword && token.type != TokenType.EOL)
                {
                    token = lexer.GetNextToken();
                }
                if (token.type == TokenType.ElseKeyword)
                {
                    return Expression();
                }
                else
                {
                    Console.WriteLine($"Expected 'else' keyword at line {token.line} and column {token.column}");
                    return null!;
                }
            }
        }
        else
        {
            Console.WriteLine($"Expected '(' after 'if' keyword at line {token.line} and column {token.column}");
            return null!;
        }
    }
    #endregion

}