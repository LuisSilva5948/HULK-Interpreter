using HULK_Interpreter;

namespace HULK_ConsoleInterface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Interpreter interpreter = new Interpreter();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("H.U.L.K. Interpreter.");
            Console.WriteLine();
            Console.WriteLine("Type a command and press Enter to execute it.");
            Console.WriteLine();
            while (true) 
            {
                Console.Write(">");
                string line = Console.ReadLine();
                if (line != null && line != "")
                {
                    interpreter.Run(line);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("No input received.");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
        }
    }
}