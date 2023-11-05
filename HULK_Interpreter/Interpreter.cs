using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public class Interpreter
    {
        //public bool hadError;

        public Interpreter() 
        {
            //hadError = false;
        }
        public void Run(string source)
        {
            try
            {
                //tokenizando
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();

                //mostrando los tokens
                /*foreach (Token token in tokens)
                {
                    Console.WriteLine(token.ToString());
                }*/
                Parser parser = new Parser(tokens);
                Expression AST = parser.Parse();
                Evaluator evaluator = new Evaluator(AST);
                object result = evaluator.EvaluateExpression(AST);
                Console.WriteLine(result);
            }
            catch (Error error)
            {
                Console.WriteLine(error.Report());
                return;
            }
            
            
        }
    }
}
