// Author: ARKye03
// GitHub: https://github.com/ARKye03/mini_kompiler
// License: MIT

namespace mini_compiler;
//Here I handle the io, with the console user interface(c_ui).
class c_ui
{
    static void Main(string[] args)
    {
        while (true)
        {
            // Read source code
            Console.Write("> ");
            string? sourceCode = Console.ReadLine();
            // Source code is null or empty
            if (sourceCode == "" || sourceCode == null)
            {
                Console.WriteLine("Error: source code is null \nPress any key to repeat...");
                Console.ReadKey();
                continue;
            }
            else
            {
                // Exit command
                if (sourceCode == "exit")
                {
                    break;
                }
                // Clear command
                if (sourceCode == "clear")
                {
                    Console.Clear();
                    continue;
                }
                // Source code does not end with ';'
                if (!sourceCode.EndsWith(";"))
                {
                    Console.WriteLine("Error: source code does not end with ';' -_- \nPress any key to repeat...");
                    Console.ReadKey();
                    continue;
                }
                // Run interpreter
                Interpreter interpreter = new Interpreter(sourceCode!);
                interpreter.Run();
            }
        }
    }
}