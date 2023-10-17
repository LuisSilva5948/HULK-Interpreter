using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public enum TokenType
    {

        Keyword,

        //Literal
        Identifier, Number, String, Boolean,

        // Single-character tokens.
        Left_Paren, Right_Paren,
        Left_Brace, Right_Brace,
        Semicolon, Comma,

        //Operators
        Plus, Minus, Times, Divide,
        Not, Not_Equal,
        Equal, Equal_Equal,
        Greater, Greater_Equal,
        Less, Less_Equal,
        And, Or, Concat,
        Lambda,

        //End of Line
        EOL
    }
}
