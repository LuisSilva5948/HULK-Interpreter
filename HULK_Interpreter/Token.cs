﻿using System;
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
        public int Line { get; }

        public Token(TokenType type, string lexeme, string literal)
        {
            Type = type;        //tipo
            Lexeme = lexeme;    //valor
            Literal = literal;  //id
        }

        public override string ToString()
        {
            return Type + " " + Literal + " " + Lexeme;
        }
    }
}