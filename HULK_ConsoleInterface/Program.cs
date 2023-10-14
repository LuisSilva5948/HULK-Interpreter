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
                string line = Console.ReadLine().ToString();
                if (line != null)
                {
                    interpreter.Run(line);
                    interpreter.hadError = false;
                }
                else Console.WriteLine("No input received.");
            }
        }
    }
}