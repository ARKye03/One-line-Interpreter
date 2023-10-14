// Author: ARKye03
// GitHub: https://github.com/ARKye03/mini_kompiler
// License: MIT
using System.Text.Json;

namespace mini_compiler;
//Aqui manejo la entrada y salida del usuario
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
    //Funcion principal que se encarga de la interfaz de usuario
    static void Main(string[] args)
    {
        if (SayBye())
            SayHello();
        while (true)
        {
            // Lee el codigo fuente
            Console.Write("> ");
            string? sourceCode = Console.ReadLine();
            // Si el codigo fuente es nulo, repetir
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
                    case "exit": // exit: salir del programa
                        return;
                    case "clear": // clear: limpiar la consola
                        Console.Clear();
                        continue;
                    case "say_bye": // say_bye: no mostrar el menu de bienvenida
                        settings.SetSayHello(false);
                        continue;
                    case "say_hello": // say_hello: mostrar el menu de bienvenida
                        settings.SetSayHello(true);
                        continue;
                }
                // Codigo fuente no termina con ';'
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
    //Funcion que se encarga de mostrar el menu de bienvenida
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