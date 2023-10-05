namespace mini_compiler;

public class Interpreter
{
    public Lexer lexer;
    public List<DFunction> functions;

    public Interpreter(string sourceCode)
    {
        lexer = new Lexer(sourceCode);
    }
    public void Run()
    {
        functions = FunctionToken.functions;
        Token token;
        //while ((token = lexer.get_next_token()).type != TokenType.EOF)
        //{
        token = lexer.get_next_token();
        if (token.type == TokenType.PrintKeyword)
        {
            // Analizar el contenido entre paréntesis y ejecutar la acción correspondiente
            var nextToken = lexer.get_next_token();
            if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
            {
                // Evaluar la expresión dentro del print y obtener el resultado
                var expressionValue = expression();

                // Verificar que se cierre el paréntesis
                nextToken = lexer.get_next_token();
                if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
                {
                    Console.WriteLine($"Expected ')' after value inside print statement at line {nextToken.line} and column {nextToken.column}");
                }
                else
                {
                    // Imprimir el resultado de la expresión evaluada
                    Console.WriteLine(expressionValue);
                }
            }
            else
            {
                Console.WriteLine($"Expected '(' after 'print' keyword at line {nextToken.line} and column {nextToken.column}");
            }
        }

        else if (token.type == TokenType.LetKeyword)
        {
            // Procesar la asignación de valores a variables
            assignment();
        }
        else if (token.type == TokenType.IfKeyword)
        {
            // Procesar la instrucción condicional
            Conditional();
        }

    }
    private void FuncParser(DFunction df)
    {
        var expression = df.expression;
        List<string> pars = df.parameters;


    }
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
            bool comparisonResult = false;
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

    private Dictionary<string, object> variables = new Dictionary<string, object>();
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
    private void statement()
    {
        var token = lexer.get_next_token();

        if (token.type == TokenType.PrintKeyword)
        {
            // Analizar el contenido entre paréntesis y ejecutar la acción correspondiente
            var nextToken = lexer.get_next_token();
            if (nextToken.type == TokenType.Punctuation && nextToken.value == "(")
            {
                // Evaluar la expresión dentro de 'print(...)'
                var expressionValue = expression();

                // Verificar que se cierre el paréntesis
                nextToken = lexer.get_next_token();
                if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
                {
                    Console.WriteLine($"Expected ')' after value inside print statement at line {nextToken.line} and column {nextToken.column}");
                }

                Console.WriteLine(expressionValue); // Imprimir el resultado
            }
            else
            {
                Console.WriteLine($"Expected '(' after 'print' keyword at line {nextToken.line} and column {nextToken.column}");
            }
        }
        else if (token.type == TokenType.LetKeyword)
        {
            assignment();
        }
        else if (token.type == TokenType.IfKeyword)
        {
            Conditional();
        }
        // Agregar más lógica para otros tipos de instrucciones si es necesario
        else if (token.type == TokenType.EOF)
        {
            return; // Fin del programa
        }
        else
        {
            Console.WriteLine($"Invalid statement at line {token.line} and column {token.column}");
        }
        // Verificar si hay un punto y coma y avanzar al siguiente statement
        token = lexer.get_next_token(); // Reutilizar la variable 'token'
        if (token.type == TokenType.EOL)
        {
            statement(); // Siguiente statement
        }
        else
        {
            lexer.unget_token(token); // Devolver el token para que se analice en la siguiente iteración
        }
    }
    private object BinaryOperation(object left, Token operatorToken, object right)
    {
        if (operatorToken.type == TokenType.Operator)
        {
            if (operatorToken.value == "+")
            {
                if (left is float && right is float)
                {
                    return (float)left + (float)right;
                }
                else if (left is string && right is string)
                {
                    return (string)left + (string)right;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '+' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "-")
            {
                if (left is float && right is float)
                {
                    return (float)left - (float)right;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '-' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "@")
            {
                if (left is string && right is string)
                {
                    return (string)left + (string)right;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '@' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "*")
            {
                if (left is float && right is float)
                {
                    return (float)left * (float)right;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '*' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "^")
            {
                if (left is float && right is float)
                {
                    return Math.Pow((float)left, (float)right);
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '^' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "/")
            {
                if (left is float && right is float)
                {
                    return (float)left / (float)right;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '/' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            else if (operatorToken.value == "%")
            {
                if (left is float && right is float)
                {
                    return (float)left % (float)right;
                }
                else
                {
                    Console.WriteLine($"Invalid operands for '%' operator at line {operatorToken.line} and column {operatorToken.column}");
                    return null!;
                }
            }
            //else if (operatorToken.value == "==")
            //{
            //    return left == right;
            //}
            //else if (operatorToken.value == "!=")
            //{
            //    return left != right;
            //}
            //else if (operatorToken.value == ">")
            //{
            //    if (left is float && right is float)
            //    {
            //        return (float)left > (float)right;
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Invalid operands for '>' operator at line {operatorToken.line} and column {operatorToken.column}");
            //        return null!;
            //    }
            //}
            //else if (operatorToken.value == "<")
            //{
            //    if (left is float && right is float)
            //    {
            //        return (float)left < (float)right;
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Invalid operands for '<' operator at line {operatorToken.line} and column {operatorToken.column}");
            //        return null!;
            //    }
            //}
            //else if (operatorToken.value == ">=")
            //{
            //    if (left is float && right is float)
            //    {
            //        return (float)left >= (float)right;
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Invalid operands for '>=' operator at line {operatorToken.line} and column {operatorToken.column}");
            //        return null!;
            //    }
            //}
            //else if (operatorToken.value == "<=")
            //{
            //    if (left is float && right is float)
            //    {
            //        return (float)left <= (float)right;
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Invalid operands for '<=' operator at line {operatorToken.line} and column {operatorToken.column}");
            //        return null!;
            //    }
            //}
            //else if (operatorToken.value == "&&")
            //{
            //    if (left is bool && right is bool)
            //    {
            //        return (bool)left && (bool)right;
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Invalid operands for '&&' operator at line {operatorToken.line} and column {operatorToken.column}");
            //        return null!;
            //    }
            //}
            //else if (operatorToken.value == "||")
            //{
            //    if (left is bool && right is bool)
            //    {
            //        return (bool)left || (bool)right;
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Invalid operands for '||' operator at line {operatorToken.line} and column {operatorToken.column}");
            //        return null!;
            //    }
            //}
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
    private object expression()
    {
        var left = term();

        while (true)
        {
            var token = lexer.get_next_token();

            // Verificar si el operador es '@'
            if (token.type == TokenType.Operator && token.value == "@")
            {
                var right = term();
                left = ConcatenateValues(left, right);
            }
            else if (token.type == TokenType.Operator && (token.value == "+" || token.value == "-"))
            {
                var right = term();
                left = BinaryOperation(left, token, right);
            }
            else
            {
                // Si no es un operador, devolver el token al lexer para que pueda ser procesado en la siguiente iteración
                lexer.unget_token(token);
                return left;
            }
        }
    }
    private object term()
    {
        var left = power();

        while (true)
        {
            var token = lexer.get_next_token();

            if (token.type != TokenType.Operator || (token.value != "*" && token.value != "/" && token.value != "%"))
            {
                lexer.unget_token(token);
                return left;
            }

            var right = power();
            left = BinaryOperation(left, token, right);
        }
    }
    private object power()
    {
        var left = primary();

        while (true)
        {
            var token = lexer.get_next_token();

            if (token.type != TokenType.Operator || token.value != "^")
            {
                lexer.unget_token(token);
                return left;
            }

            var right = primary();
            left = BinaryOperation(left, token, right);
        }
    }
    private object factor()
    {
        var left = primary();

        while (true)
        {
            var token = lexer.get_next_token();

            if (token.type != TokenType.Operator || (token.value != "+" && token.value != "-"))
            {
                lexer.unget_token(token);
                return left;
            }

            var right = primary();
            left = BinaryOperation(left, token, right);
        }
    }
    private object primary()
    {
        var token = lexer.get_next_token();

        foreach (var func in functions)
        {
            if (token.value == func.value)
            {
                return func.expression;
            }
        }
        if (token.type == TokenType.Number)
        {
            return float.Parse(token.value);
        }
        else if (token.type == TokenType.StringLiteral)
        {
            return token.value;
        }
        else if (token.type == TokenType.Identifier)
        {
            if (variables.ContainsKey(token.value))
            {
                return variables[token.value];
            }
            Console.WriteLine($"Undefined variable '{token.value}' at line {token.line} and column {token.column}");
            return null!;
        }
        else if (token.type == TokenType.Punctuation && token.value == "(")
        {
            var expressionValue = expression();
            var nextToken = lexer.get_next_token();
            if (nextToken.type != TokenType.Punctuation || nextToken.value != ")")
            {
                Console.WriteLine($"Expected ')' after expression at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
            return expressionValue;
        }
        else if (token.type == TokenType.LetKeyword && token.value == "let")
        {
            var variableToken = lexer.get_next_token();
            if (variableToken.type != TokenType.Identifier)
            {
                Console.WriteLine($"Expected identifier after 'let' keyword at line {variableToken.line} and column {variableToken.column}");
                return null!;
            }
            var equalsToken = lexer.get_next_token();
            if (equalsToken.type != TokenType.Operator || equalsToken.value != "=")
            {
                Console.WriteLine($"Expected '=' after identifier in 'let' expression at line {equalsToken.line} and column {equalsToken.column}");
                return null!;
            }
            var value = expression();
            variables[variableToken.value] = value;
            var nextToken = lexer.get_next_token();
            if (nextToken.type != TokenType.InKeyword || nextToken.value != "in")
            {
                Console.WriteLine($"Expected 'in' keyword after expression in 'let' expression at line {nextToken.line} and column {nextToken.column}");
                return null!;
            }
            var expressionValue = expression();
            return expressionValue;
        }
        else if (token.type == TokenType.Operator && token.value == "-")
        {
            var nextToken = lexer.get_next_token();
            if (nextToken.type == TokenType.Number)
            {
                return -float.Parse(nextToken.value);
            }
            Console.WriteLine($"Expected number after '-' operator at line {nextToken.line} and column {nextToken.column}");
            return null!;
        }
        else
        {
            Console.WriteLine($"Invalid expression at line {token.line} and column {token.column}");
            return null!;
        }
    }
    private object ConcatenateValues(object left, object right)
    {
        // Si ambos valores son cadenas, concatenarlos y devolver el resultado
        if (left is string && right is string)
        {
            return (string)left + (string)right;
        }
        // Si alguno de los valores es una cadena, convertir el otro valor a cadena y concatenarlos
        if (left is string)
        {
            return (string)left + right.ToString();
        }

        if (right is string)
        {
            return left.ToString() + (string)right;
        }
        // Si ninguno de los valores es cadena, intentar sumar los números
        if (left is float && right is float)
        {
            return (float)left + (float)right;
        }
        // Si no se pueden concatenar los valores, mostrar un mensaje de error
        Console.WriteLine($"Invalid operands for '@' operator at line ... and column ...");
        return null!;
    }
}