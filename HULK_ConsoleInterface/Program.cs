using HULK_Interpreter;

namespace HULK_ConsoleInterface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Interpreter interpreter = new Interpreter();
            do
            {
                Console.Write(">");
                string line = Console.ReadLine().ToString();
                if (line != null)
                {
                    interpreter.Run(line);
                }
                else throw new Exception("No input received.");
            } while (true);
        }
    }
}