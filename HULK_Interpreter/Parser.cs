﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    internal class Parser
    {
        private readonly List<Token> tokens;
        private int currentPos;
        private Token currentToken;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            currentPos = 0;
            currentToken = tokens[0];
        }
    }
}
