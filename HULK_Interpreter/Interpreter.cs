using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public class Interpreter
    {
        private List<Error> errors;
        public bool hadError;

        public Interpreter() 
        {
            hadError = false;
        }
        public void Run(string source)
        {
            //tokenizando
            Lexer lexer = new Lexer(source);
            List<Token> tokens = lexer.ScanTokens();

            if (!hadError)
            {

            }

            //mostrando los tokens
            foreach (Token token in tokens)
            {
                Console.WriteLine(token.ToString());
            }
        }
    }
}
