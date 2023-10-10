namespace mini_compiler;

public partial class Interpreter
{
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
                    return Math.Pow(l, r);
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
    private static object ConcatenateValues(object left, object right)
    {
        // Si ambos valores son cadenas, concatenarlos y devolver el resultado
        if (left is string l && right is string r)
        {
            return l + r;
        }
        // Si alguno de los valores es una cadena, convertir el otro valor a cadena y concatenarlos
        if (left is string ls)
        {
            return ls + right.ToString();
        }

        if (right is string rs)
        {
            return left.ToString() + rs;
        }
        // Si ninguno de los valores es cadena, intentar sumar los números
        if (left is float lf && right is float rf)
        {
            return lf + rf;
        }
        // Si no se pueden concatenar los valores, mostrar un mensaje de error
        Console.WriteLine($"Invalid operands for '@' operator at line ... and column ...");
        return null!;
    }
}