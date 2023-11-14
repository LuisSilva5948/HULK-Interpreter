﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public class Interpreter
    {
        public Interpreter() 
        {
        }
        public void Run(string source)
        {
            try
            {
                Lexer lexer = new Lexer(source);
                List<Token> tokens = lexer.ScanTokens();

                //mostrando los tokens
                /*foreach (Token token in tokens)
                {
                    Console.WriteLine(token.ToString());
                }*/

                Parser parser = new Parser(tokens);
                Expression AST = parser.Parse();
                Evaluator evaluator = new Evaluator();
                object result = evaluator.Evaluate(AST);
                Console.WriteLine(result);
            }
            catch (Error error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error.Report());
                Console.ForegroundColor = ConsoleColor.Green;
                return;
            }
        }
    }
}
