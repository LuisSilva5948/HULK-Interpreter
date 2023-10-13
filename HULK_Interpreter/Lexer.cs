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
        
        public List<Token> GetTokens()
        {
            while(!isAtEnd()){
                start = current;
                ScanToken();
            }
            return tokens;
        }
        public Token ScanToken()
        {
            //implementar scantoken
            return tokens[start];
        }
        public bool isAtEnd() => current >= source.Length;
    }
    
}