using System.ComponentModel.Design;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace HULK_Interpreter
{
    public class Lexer
    {
        private readonly string source;
        private List<Token> tokens;
        public List<Error> errors { get; private set; }
        private int startofLexeme;
        private int currentPos;
        //startofLexeme apunta al primer caracter en el lexema siendo escaneado,
        //current apunta al caracter actualmente siendo considerado.

        public Lexer(string source)
        {
            this.source = source;
            errors = new List<Error>();
            tokens = new List<Token>();
            startofLexeme = 0;
            currentPos = 0;
        }
        
        public List<Token> ScanTokens()
        {
            while(!IsAtEnd()){
                startofLexeme = currentPos;
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
                case '\0':
                //ignorar espacios en blanco
                case ' ':
                case '\r':
                case '\t':
                case '\n':
                    break;
                //buscar tokens de uno o dos caracteres
                case '(': AddToken(TokenType.Left_Paren); break;
                case ')': AddToken(TokenType.Right_Paren); break;
                case ',': AddToken(TokenType.Comma); break;
                case ';': AddToken(TokenType.Semicolon); break;
                case '-': AddToken(TokenType.Minus); break;
                case '+': AddToken(TokenType.Plus); break;
                case '*': AddToken(TokenType.Times); break;
                case '/': AddToken(TokenType.Divide); break;
                case '%': AddToken(TokenType.Module); break;
                case '^': AddToken(TokenType.Power); break;
                case '@': AddToken(TokenType.Concat); break;
                case '&': AddToken(TokenType.And); break;
                case '|': AddToken(TokenType.Or); break;
                case '!': AddToken(Match('=') ? TokenType.Not_Equal : TokenType.Not); break;
                case '<': AddToken(Match('=') ? TokenType.Less_Equal : TokenType.Less); break;
                case '>': AddToken(Match('=') ? TokenType.Greater_Equal : TokenType.Greater); break;
                case '=':
                    if (Match('>')) AddToken(TokenType.Lambda);
                    else AddToken(Match('=') ? TokenType.Equal_Equal : TokenType.Equal); break;
                case '\"': ScanStringValue(); break;
                //buscar numero, string o devolver error
                default:
                    if (char.IsDigit(c))
                    {
                        ScanNumber();
                        break;
                    }
                    if (char.IsLetter(c) || c == '_')
                    {
                        ScanString();
                        break;
                    }
                    errors.Add(new Error(ErrorType.Lexical, c + " is not a supported character."));
                    break;
            }
        }

        private void AddToken(TokenType tokentype) 
        {
            AddToken(tokentype, null);
        }
        private void AddToken(TokenType tokentype, Object literal)
        {
            string lexeme = GetLexeme();
            tokens.Add(new Token(tokentype, lexeme, literal));
        }

        //comprobador de si matchea el caracter actual con el que espera que le siga, si matchea avanza
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (source[currentPos] != expected) return false;
            currentPos++;
            return true;
        }
        //retorna el char actual
        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return source[currentPos];
        }

        private bool IsAtEnd() => currentPos >= source.Length;
        private char Advance() => source[currentPos++];
        private string GetLexeme() => source.Substring(startofLexeme, currentPos - startofLexeme);

        private void ScanNumber()
        {
            int dotCounter = 0;
            bool isvalidnumber = true;
            while (char.IsLetterOrDigit(Peek()) || Peek() == '.'){
                if (Peek() == '.') dotCounter++;
                if (char.IsLetter(Peek())) isvalidnumber = false;
                Advance();
            }
            if (dotCounter > 1 || !isvalidnumber) errors.Add(new Error(ErrorType.Lexical, GetLexeme() + " is not a valid token."));
            else AddToken(TokenType.Number, double.Parse(GetLexeme()));
        }
        private void ScanString()
        {
            while (char.IsLetterOrDigit(Peek()) || Peek() == '_')
            {
                Advance();
            }
            string lexeme = GetLexeme();
            switch(lexeme)
            {
                case "let": AddToken(TokenType.Let, lexeme); break;
                case "in": AddToken(TokenType.In, lexeme); break;
                case "if": AddToken(TokenType.If, lexeme); break;
                case "else": AddToken(TokenType.Else, lexeme); break;
                case "print": AddToken(TokenType.Print, lexeme); break;
                case "function": AddToken(TokenType.Function, lexeme); break;
                case "true":
                case "false":
                    AddToken(TokenType.Boolean, bool.Parse(lexeme)); break;
                case "PI": AddToken(TokenType.Number, Math.PI); break;
                case "e": AddToken(TokenType.Number, Math.E); break;
                case "sen": AddToken(TokenType.Sen, lexeme); break;
                case "cos": AddToken(TokenType.Cos, lexeme); break;
                default:
                    string possiblelexeme = lexeme.ToLower();
                    if (possiblelexeme == lexeme)
                    {
                        AddToken(TokenType.Identifier, lexeme);
                        break;
                    }
                    errors.Add(new Error(ErrorType.Lexical, '\"' + lexeme + "\" is not a valid identifier."));
                    break;
            }
        }
        private void ScanStringValue()
        {
            //tratando de no llevarme la comilla como parte del
            if (Peek() == '\"')
            {
                Advance();
                AddToken(TokenType.String, "");
                return;
            }
            else if (IsAtEnd())
            {
                errors.Add(new Error(ErrorType.Lexical, "( \" ) is not a valid token."));
                return;
            }
            int startIndex = currentPos;
            while (Peek() != '\"')
            {
                if (IsAtEnd())
                {
                    errors.Add(new Error(ErrorType.Lexical, "( \" ) expected."));
                    return;
                }
                Advance();
            }
            if (Peek() == '\"')
                Advance();
            int endIndex = currentPos - 1;
            string literal = source.Substring(startIndex, endIndex - startIndex);
            AddToken(TokenType.String, literal);
        }
    }
    
}