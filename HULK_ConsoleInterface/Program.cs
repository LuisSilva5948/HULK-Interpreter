using HULK_Interpreter;

namespace HULK_ConsoleInterface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            for(; ;)
            {
                Console.WriteLine(">");
                string line = Console.ReadLine().ToString();
                if (line != null)
                {
                    Lexer lexer = new Lexer(line);
                }
                else throw new Exception("No input received.");
            }
        }
    }
}