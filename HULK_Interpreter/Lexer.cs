using System.Text.RegularExpressions;

namespace HULK_Interpreter
{
    public class Lexer
    {
        private readonly string source;
        private List<Token> tokens = new List<Token>();
        private int startofLexeme = 0;
        private int current = 0;
        //startofLexeme apunta al primer caracter en el lexema siendo escaneado,
        //current apunta al caracter actualmente siendo considerado.

        public Lexer(string source)
        {
            this.source = source;
        }
        
        public List<Token> ScanTokens()
        {
            while(!IsAtEnd()){
                startofLexeme = current;
                ScanToken();
            }
            tokens.Add(new Token(TokenType.EOL,"",null));
            return tokens;
        }
        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.Left_Paren); break;
                case ')': AddToken(TokenType.Right_Paren); break;
                case '-': AddToken(TokenType.Minus); break;
                case '+': AddToken(TokenType.Plus); break;
                case ';': AddToken(TokenType.Semicolon); break;
                default:
                    Console.WriteLine(new Error(ErrorType.Lexical, "Unexpected character: " + c).ToString());
                    break;
            }
        }
        //los agregadores de tokens a la lista
        private void AddToken(TokenType tokentype) => AddToken(tokentype, null);
        private void AddToken(TokenType tokentype, Object literal)
        {
            string lexeme = source.Substring(startofLexeme, current - startofLexeme);
            tokens.Add(new Token(tokentype, lexeme, literal));
        }
        //bool para si hemos terminado de escanear todo
        private bool IsAtEnd() => current >= source.Length;
        //retorna el proximo caracter
        private char Advance() => source[current++];
    }
    
}