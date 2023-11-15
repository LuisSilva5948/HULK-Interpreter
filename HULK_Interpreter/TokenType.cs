using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public enum TokenType
    {

        // Keywords
        LET,
        IN,
        IF,
        ELSE,
        PI,
        EULER,
        FUNCTION,
        PRINT,

        // Variables
        IDENTIFIER,
        NUMBER,
        STRING,
        BOOLEAN,

        // Separators
        LEFT_PAREN,
        RIGHT_PAREN,
        SEMICOLON,
        COMMA,

        // Operators
        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        MODULUS,
        POWER,
        AND,
        OR,
        NOT,
        NOT_EQUAL,
        EQUAL,
        ASSING,
        GREATER,
        GREATER_EQUAL,
        LESS,
        LESS_EQUAL,
        CONCAT,
        LAMBDA,

        // End of File
        EOF
    }
}
