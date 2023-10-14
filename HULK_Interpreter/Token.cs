using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public object Literal { get; }

        public Token(TokenType type, string lexeme, Object literal)
        {
            Type = type;        //tipo del token
            Lexeme = lexeme;    //string del token
            Literal = literal;  //valor del token
        }

        public override string ToString()
        {
            return Type + " " + Lexeme + " " + Literal;
        }
    }
}
