namespace mini_compiler;

public partial class Interpreter
{
    /// <summary>
    /// Parses an expression and returns the result of the evaluation.
    /// Actually, this is part "BinaryExpression", in other cases, but it just handles operations
    /// </summary>
    /// <param name="left"></param>
    /// <param name="operatorToken"></param>
    /// <param name="right"></param>
    /// <returns>Evaluated operations</returns>
    private static object BinaryOperation(object left, Token operatorToken, object right)
    {
        if (operatorToken.type == TokenType.Operator)
        {
            if (operatorToken.value == "+")
            {
                if (left is float l && right is float r)
                {
                    return l + r;
                }
                else if (left is string ls && right is string rs)
                {
                    return ls + rs;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '+' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "-")
            {
                if (left is float l && right is float r)
                {
                    return l - r;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '-' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "@")
            {
                if (left is string l && right is string r)
                {
                    return l + r;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '@' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "*")
            {
                if (left is float l && right is float r)
                {
                    return l * r;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '*' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "^")
            {
                if (left is float l && right is float r)
                {
                    return Convert.ToSingle(Math.Pow(l, r));
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '^' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "/")
            {
                if (left is float l && right is float r)
                {
                    return l / r;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '/' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "%")
            {
                if (left is float l && right is float r)
                {
                    return l % r;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '%' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else
            {
                Console.WriteLine($"Invalid operator '{operatorToken.value}' at line {operatorToken.line} and column {operatorToken.column}");
                return null!;
            }
        }
        else
        {
            Console.WriteLine($"Expected operator at line {operatorToken.line} and column {operatorToken.column}");
            return null!;
        }
    }
    /// <summary>
    /// This is used for @ operator
    /// </summary>
    /// <returns>Concatenated strings or + numbers</returns>
    private static object ConcatenateValues(object left, object right)
    {
        // If both values ​​are strings, concatenate them and return the result
        if (left is string l && right is string r)
        {
            return l + r;
        }
        // If any of the values ​​is a string, convert the other value to a string and concatenate them
        if (left is string ls)
        {
            return ls + right.ToString();
        }

        if (right is string rs)
        {
            return left.ToString() + rs;
        }
        // If none of the values ​​is a string, try adding the numbers
        if (left is float lf && right is float rf)
        {
            return lf + rf;
        }
        // If values ​​cannot be concatenated, display an error message
        Console.WriteLine($"Invalid operands for '@' operator at line ... and column ...");
        return null!;
    }
}