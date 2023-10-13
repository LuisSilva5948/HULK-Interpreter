using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public enum TokenType
    {
        //Literal
        Identifier, Number, String,

        // Single-character tokens.
        Left_Paren, Right_Paren,

        //Operators
        Plus, Minus,

        //End of Line
        EOL
    }
}
