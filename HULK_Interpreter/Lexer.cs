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
        private Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>{
            {"let", TokenType.LET},
            {"in", TokenType.IN},
            {"print", TokenType.PRINT},
            {"function", TokenType.FUNCTION},
            {"if", TokenType.IF},
            {"else", TokenType.ELSE},
            {"true", TokenType.BOOLEAN},
            {"false", TokenType.BOOLEAN},
            {"PI", TokenType.NUMBER},
            {"e", TokenType.NUMBER},
            {"sen", TokenType.SEN},
            {"cos", TokenType.COS},
            {"log", TokenType.LOG}
        };

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
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case ',': AddToken(TokenType.COMMA); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case '*': AddToken(TokenType.TIMES); break;
                case '/': AddToken(TokenType.DIVIDE); break;
                case '%': AddToken(TokenType.MODULE); break;
                case '^': AddToken(TokenType.POWER); break;
                case '@': AddToken(TokenType.CONCAT); break;
                case '&': AddToken(TokenType.AND); break;
                case '|': AddToken(TokenType.OR); break;
                case '!': AddToken(Match('=') ? TokenType.NOT_EQUAL : TokenType.NOT); break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '=':
                    if (Match('>')) AddToken(TokenType.LAMBDA);
                    else AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
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
            else AddToken(TokenType.NUMBER, double.Parse(GetLexeme()));
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
                case "let": AddToken(TokenType.LET, lexeme); break;
                case "in": AddToken(TokenType.IN, lexeme); break;
                case "print": AddToken(TokenType.PRINT, lexeme); break;
                case "function": AddToken(TokenType.FUNCTION, lexeme); break;
                case "if": AddToken(TokenType.IF, lexeme); break;
                case "else": AddToken(TokenType.ELSE, lexeme); break;
                case "true":
                case "false":
                    AddToken(TokenType.BOOLEAN, bool.Parse(lexeme)); break;
                case "PI": AddToken(TokenType.NUMBER, Math.PI); break;
                case "e": AddToken(TokenType.NUMBER, Math.E); break;
                case "sen": AddToken(TokenType.SEN, lexeme); break;
                case "cos": AddToken(TokenType.COS, lexeme); break;
                case "log": AddToken(TokenType.LOG, lexeme); break;
                default:
                    if (keywords.ContainsKey(lexeme.ToLower()))
                        errors.Add(new Error(ErrorType.Lexical, '\"' + lexeme + "\" is not a valid identifier."));
                    else
                        AddToken(TokenType.IDENTIFIER, lexeme);
                    break;
            }
        }
        private void ScanStringValue()
        {
            //tratando de no llevarme la comilla como parte del
            if (Peek() == '\"')
            {
                Advance();
                AddToken(TokenType.STRING, "");
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
            AddToken(TokenType.STRING, literal);
        }
    }
    
}