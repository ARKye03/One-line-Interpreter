// Author: ARKye03
// GitHub: https://github.com/ARKye03/mini_kompiler
// License: MIT
using System.Text.Json;

namespace mini_compiler;
//Here I handle the IO of the user
public class c_ui
{
    #region Test
    public static void Test(string sourceCode)
    {
        // Run interpreter
        Interpreter interpreter = new(sourceCode!);
        interpreter.Run();
    }
    #endregion
    //Main function that is responsible for the user interface
    static void Main(string[] args)
    {
        if (SayBye())
            SayHello();
        while (true)
        {
            // Read the source code
            Console.Write("> ");
            string? sourceCode = Console.ReadLine();
            // If the source code is null, repeat
            if (string.IsNullOrEmpty(sourceCode))
            {
                Console.WriteLine("Error: source code is null \nPress any key to repeat...");
                Console.ReadKey();
                continue;
            }
            else
            {
                switch (sourceCode)
                {
                    // Comandos
                    case "exit": // exit: exit the program
                        return;
                    case "info":
                        FunctionsInfo();
                        continue;
                    case "clear": // clear: clean the console
                        Console.Clear();
                        continue;
                    case "say_bye": // say_bye: do not show the welcome menu
                        settings.SetSayHello(false);
                        continue;
                    case "say_hello": // say_hello: show the welcome menu
                        settings.SetSayHello(true);
                        continue;
                }
                // Source code does not end with ';'
                if (!sourceCode.EndsWith(";"))
                {
                    Console.WriteLine($"Expected ';' at column {sourceCode.Length}.");
                    continue;
                }
                // Run interpreter
                Interpreter interpreter = new Interpreter(sourceCode!);
                interpreter.Run();
            }
        }
    }

    private static void FunctionsInfo()
    {
        List<string> functionDescriptions = new List<string>
        {
            "Sin(x):    | Returns the sine of x, where x is in radians.            | Example: Sin(0) returns 0.",
            "Cos(x):    | Returns the cosine of x, where x is in radians.          | Example: Cos(0) returns 1.",
            "Tan(x):    | Returns the tangent of x, where x is in radians.         | Example: Tan(0) returns 0.",
            "Log(x):    | Returns the natural logarithm (base e) of x.             | Example: Log(1) returns 0.",
            "Ln(x):     | Returns the natural logarithm (base e) of x.             | Example: Ln(1) returns 0.",
            "Sqrt(x):   | Returns the square root of x.                            | Example: Sqrt(4) returns 2.",
            "Abs(x):    | Returns the absolute value of x.                         | Example: Abs(-5) returns 5.",
            "Pow(x, y): | Returns x raised to the power of y.                      | Example: Pow(2, 3) returns 8.",
            "Exp(x):    | Returns e raised to the power of x.                      | Example: Exp(1) returns approximately 2.71828.",
            "Floor(x):  | Returns the largest integer less than or equal to x.     | Example: Floor(1.5) returns 1.",
            "Ceil(x):   | Returns the smallest integer greater than or equal to x. | Example: Ceil(1.5) returns 2.",
            "Round(x):  | Rounds x to the nearest integer.                         | Example: Round(1.5) returns 2.",
            "Rand(min, max):    | Returns a random integer between min (inclusive) and max (exclusive). | Example: Rand(10, 20) returns a random number between 10 and 20.",
            "Factorial(x):      | Returns the factorial of x.                           | Example: Factorial(5) returns 120.",
            "Fibonacci(x):      | Returns the xth number in the Fibonacci sequence.     | Example: Fibonacci(5) returns 5.",
            "IsPrime(x):        | Returns true if x is a prime number, false otherwise. | Example: IsPrime(5) returns true.",
            "IsEven(x):         | Returns true if x is an even number, false otherwise. | Example: IsEven(5) returns false.",
            "IsDivisible(x, y): | Returns true if x is divisible by y, false otherwise. | Example: IsDivisible(10, 5) returns true.",
            "IsPalindrome(x):   | Returns true if x is a palindrome, false otherwise.   | Example: IsPalindrome(\"radar\") returns true."
        };

        foreach (string description in functionDescriptions)
        {
            string[] parts = description.Split(new[] { ':' }, 2); // Split the description into two parts at the first colon

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(parts[0]); // Print the function name in red

            Console.ResetColor();
            Console.WriteLine(parts.Length > 1 ? ":" + parts[1] : ""); // Print the rest of the description in the default color
        }
    }

    //Function that is responsible for displaying the welcome menu
    #region SayHello
    private static void SayHello()
    {
        SayBye();
        string[] lines = {
            "          <- Welcome to my mini_kompiler! ->               \n",
            "----------------------------------------------------------   ",
            "  You can type and send:                              \n",
            "|//////  ////// //////  ////// ///////      ////// //////|   ",
            "    - \"clear\" to clear the console                  \n",
            "|  //      //     //      //     //           //    ///  |   ",
            "                                                           \n",
            "|  //////////     //      //     //           // ///     |   ",
            "    - \"exit\" to exit the console                    \n",
            "|  //      //     //      //     //     //    //   ///   | \n",
            "|//////  //////    ////////    ///////////  //////  /////|   ",
            "    - \"say_hello|say_bye\" to show or not this menu\n",
            "---------------------------------------------------------- \n",
            "     <- Havana University Language for Kompilers ->        \n",
            "",
        };
        Console.ForegroundColor = ConsoleColor.Green;
        for (int i = 0; i < lines.Length; i++)
        {
            if (i == 2 || i == 4 || i == 8 || i == 11)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.Write(lines[i]);
            Console.ForegroundColor = ConsoleColor.Green;
        }
        Console.ResetColor();
    }
    private static Settings settings = new Settings();
    private static bool SayBye()
    {
        // Check if the settings file exists
        if (!File.Exists(".settings/settings.json"))
        {
            // Create a new settings object
            {
                settings.say_hello = true;
            };
            // Serialize the settings to JSON
            string json = JsonSerializer.Serialize(settings);
            // Write the JSON to the file
            Directory.CreateDirectory(".vscode");
            File.WriteAllText(".settings/settings.json", json);
            // Return true since say_hello is true by default
            return true;
        }
        else
        {
            // Load the settings from the JSON file
            string json = File.ReadAllText(".settings/settings.json");
            Settings? settings = JsonSerializer.Deserialize<Settings>(json);
            // Return the value of the say_hello property
            return settings!.say_hello;
        }
    }
    private class Settings
    {
        public bool say_hello { get; set; }

        public void SetSayHello(bool value)
        {
            say_hello = value;
            // Serialize the settings to JSON
            string json = JsonSerializer.Serialize(this);
            // Write the JSON to the file
            Directory.CreateDirectory(".settings");
            File.WriteAllText(".settings/settings.json", json);
        }
    }
    #endregion
}