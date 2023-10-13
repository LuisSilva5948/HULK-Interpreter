using HULK_Interpreter;

namespace HULK_ConsoleInterface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.Write(">");
                string line = Console.ReadLine().ToString();
                if (line != null)
                {
                    Lexer.Tokenize(line);
                }
                else throw new Exception("No input received.");
            } while (true);
        }
    }
}