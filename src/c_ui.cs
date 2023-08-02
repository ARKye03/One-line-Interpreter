//I wanna print the tokens and the expresions
namespace mini_compiler;

class c_ui
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("> ");
            string? sourceCode = Console.ReadLine();
            if (sourceCode == "exit")
            {
                break;
            }
            if (sourceCode == "clear")
            {
                Console.Clear();
                continue;
            }
            Interpreter interpreter = new Interpreter(sourceCode!);
            interpreter.Run();
        }
    }
}
