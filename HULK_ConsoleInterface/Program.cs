using HULK_Interpreter;

namespace HULK_ConsoleInterface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Interpreter interpreter = new Interpreter();
            while (true) 
            {
                Console.Write(">");
                string line = Console.ReadLine();
                if (line != null && line != "")
                {
                    interpreter.Run(line);
                }
                else Console.WriteLine("No input received.");
            }
        }
    }
}