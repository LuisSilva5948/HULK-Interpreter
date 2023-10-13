using System.Text.RegularExpressions;

namespace HULK_Interpreter
{
    public class Lexer
    {
        private readonly string source;
        private List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;

        public Lexer(string source)
        {
            this.source = source;
        }
        
        public List<Token> ScanTokens()
        {
            while(!isAtEnd()){
                start = current;
                scanToken();
            }
            tokens.Add(new Token(TokenType.EOL,"",""));
            return tokens;
        }

        public bool isAtEnd() => current >= source.Length;
        private char advance() => source[current++];
        private void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case '(': addToken(TokenType.Left_Paren); break;
                case ')': addToken(TokenType.Right_Paren); break;
                case '-': addToken(TokenType.Minus); break;
                case '+': addToken(TokenType.Plus); break;
                case ';': addToken(TokenType.Semicolon); break;
            }
        }
        private void addToken(TokenType type) => addToken(type, null);

        private void addToken(TokenType tokentype, Object literal)
        {
            string text = source[start..current];
            tokens.Add(new Token(tokentype, text, literal));
        }
    }
    
}