using System.Text.RegularExpressions;

namespace HULK_Interpreter
{
    public class Lexer
    {
        private readonly string source;
        private List<Token> tokens;
        private List<Error> errors;
        private int startofLexeme;
        private int current;
        //startofLexeme apunta al primer caracter en el lexema siendo escaneado,
        //current apunta al caracter actualmente siendo considerado.

        public Lexer(string source)
        {
            this.source = source;
            errors = new List<Error>();
            tokens = new List<Token>();
            startofLexeme = 0;
            current = 0;
        }
        
        public List<Token> ScanTokens()
        {
            while(!IsAtEnd()){
                startofLexeme = current;
                ScanToken();
            }
            tokens.Add(new Token(TokenType.EOL,"EOL",null));
            return tokens;
        }

        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                //ignorar espacios en blanco
                case ' ':
                case '\r':
                case '\t':
                case '\n':
                    break;
                //buscar tokens de uno o dos caracteres
                case '(': AddToken(TokenType.Left_Paren); break;
                case ')': AddToken(TokenType.Right_Paren); break;
                case '{': AddToken(TokenType.Left_Brace); break;
                case '}': AddToken(TokenType.Right_Brace); break;
                case ',': AddToken(TokenType.Comma); break;
                case ';': AddToken(TokenType.Semicolon); break;
                case '-': AddToken(TokenType.Minus); break;
                case '+': AddToken(TokenType.Plus); break;
                case '!': AddToken(Match('=') ? TokenType.Not_Equal : TokenType.Not); break;
                case '=': AddToken(Match('=') ? TokenType.Equal_Equal : TokenType.Equal); break;
                case '<': AddToken(Match('=') ? TokenType.Less_Equal : TokenType.Less); break;
                case '>': AddToken(Match('=') ? TokenType.Greater_Equal : TokenType.Greater); break;
                //buscar numero, string o devolver error
                default:
                    if (char.IsDigit(c))
                    {
                        ScanNumber();
                        break;
                    }
                    if (char.IsLetter(c))
                    {
                        ScanString();
                        break;
                    }
                    errors.Add(new Error(ErrorType.Lexical, c + " is not a valid token."));
                    break;
            }
        }

        //los agregadores de tokens a la lista
        private void AddToken(TokenType tokentype) 
        {
            AddToken(tokentype, null);
        }
        private void AddToken(TokenType tokentype, Object literal)
        {
            string lexeme = source.Substring(startofLexeme, current - startofLexeme);
            tokens.Add(new Token(tokentype, lexeme, literal));
        }

        //comprobador de si matchea el caracter actual con el que espera que le siga, si matchea avanza
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (source[current] != expected) return false;
            current++;
            return true;
        }

        //bool para si hemos terminado de escanear todo
        private bool IsAtEnd()
        {
            return current >= source.Length;
        }
        //retorna el proximo caracter
        private char Advance()
        {
            return source[current++];
        }

        private void ScanNumber()
        {
            while (true)
            {

            }
            return;
        }
        private void ScanString()
        {
            return;
        }
    }
    
}