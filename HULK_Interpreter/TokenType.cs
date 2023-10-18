using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public enum TokenType
    {

        //Keywords
        Let,
        In,
        Print,
        Function,
        If,
        Else,
        PI,
        Euler,
        Sen,
        Cos,
        
        //variables
        Identifier,
        Number,
        String,
        Boolean,

        // Separators
        Left_Paren,
        Right_Paren,
        Semicolon,
        Comma,

        //Operators
        Plus,
        Minus,
        Times,
        Divide,
        Module,
        Power,
        And,
        Or,
        Not,
        Not_Equal,
        Equal,
        Equal_Equal,
        Greater,
        Greater_Equal,
        Less,
        Less_Equal,
        Concat,
        Lambda,

        //End of Line
        EOL
    }
}
