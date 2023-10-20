using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private int currentPos;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            currentPos = 0;
        }

        private Token GetToken()
        {
            return tokens[currentPos];
        }

        private new TokenType GetType()
        {
            return tokens[currentPos].Type;
        }

        private string GetLexeme()
        {
            return tokens[currentPos].Lexeme;
        }
        private int Advance()
        {
            if (GetType() != TokenType.EOF)
                currentPos++;

            return currentPos - 1;
        }

        
    }
}
