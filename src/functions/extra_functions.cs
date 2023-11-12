namespace mini_compiler;
public partial class Interpreter
{
    // List of declarable functions, to be used in the program
    public List<DFunction>? functions;

    /// <summary>
    /// Represents a list of built-in mathematical functions used by the interpreter.
    /// </summary>
    public List<Functions> functions2 = new()
    {
        new("Sin", 1, args => Math.Sin(Convert.ToSingle(args[0]))),
        new("Cos", 1, args => Math.Cos(Convert.ToSingle(args[0]))),
        new("Tan", 1, args => Math.Tan(Convert.ToSingle(args[0]))),
        new("Log", 1, args => Math.Log(Convert.ToSingle(args[0]))),
        new("Ln", 1, args => Math.Log(Convert.ToSingle(args[0]), Math.E)),
        new("Sqrt", 1, args => Math.Sqrt(Convert.ToSingle(args[0]))),
        new("Abs", 1, args => Math.Abs(Convert.ToSingle(args[0]))),
        new("Pow", 2, args => Math.Pow(Convert.ToSingle(args[0]), Convert.ToSingle(args[1]))),
        new("Exp", 1, args => Math.Exp(Convert.ToSingle(args[0]))),
        new("Floor", 1, args => Math.Floor(Convert.ToSingle(args[0]))),
        new("Ceil", 1, args => Math.Ceiling(Convert.ToSingle(args[0]))),
        new("Round", 1, args => Math.Round(Convert.ToSingle(args[0]))),
        new("Rand", 2, args =>
        {
            int min = Convert.ToInt32(args[0]);
            int max = Convert.ToInt32(args[1]);
            return new Random().Next(min, max);
        }),
        new("Factorial", 1, args => Factorial(Convert.ToInt32(args[0]))),
        new("Fibonacci", 1, args => Fibonacci(Convert.ToInt32(args[0]))),
        new("IsPrime", 1, args => IsPrime(Convert.ToInt32(args[0]))),
        new("IsEven", 1, args => IsEven(Convert.ToInt32(args[0]))),
        new("IsDivisible", 2, args => IsDivisible(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]))),
        new("IsPalindrome", 1, args => IsPalindrome(args[0].ToString()!)),
    };
    public static int Factorial(int n)
    {
        if (n < 0)
            throw new ArgumentException("Negative input is not allowed for factorial.");
        return n == 0 ? 1 : n * Factorial(n - 1);
    }

    public static int Fibonacci(int n)
    {
        if (n < 0)
            throw new ArgumentException("Negative input is not allowed for Fibonacci.");
        if (n <= 1)
            return n;
        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }

    public static bool IsPrime(int n)
    {
        if (n <= 1)
            return false;
        if (n == 2)
            return true;
        if (n % 2 == 0)
            return false;
        var boundary = (int)Math.Floor(Math.Sqrt(n));
        for (int i = 3; i <= boundary; i += 2)
            if (n % i == 0)
                return false;
        return true;
    }

    public static bool IsEven(int n)
    {
        return n % 2 == 0;
    }

    public static bool IsDivisible(int n, int m)
    {
        if (m == 0)
            throw new ArgumentException("Division by zero is not allowed.");
        return n % m == 0;
    }

    public static bool IsPalindrome(string str)
    {
        var reversedStr = new string(str.Reverse().ToArray());
        return str == reversedStr;
    }
}