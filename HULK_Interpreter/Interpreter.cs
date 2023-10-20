using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public class Interpreter
    {
        public bool hadError;

        public Interpreter() 
        {
            hadError = false;
        }
        public void Run(string source)
        {
            try
            {
                //tokenizando
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();

                if (lexer.errors.Count > 0)
                {
                    foreach (Error error in lexer.errors)
                    {
                        Console.WriteLine(error.ToString());
                    }
                }
                //mostrando los tokens
                foreach (Token token in tokens)
                {
                    Console.WriteLine(token.ToString());
                }
                Parser parser = new Parser(tokens);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e}");
                return;
            }
            
            
        }
    }
}
