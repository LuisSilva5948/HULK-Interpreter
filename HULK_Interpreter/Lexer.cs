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
        private int startofLexeme;
        private int currentPos;
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
            {"E", TokenType.NUMBER},
        };

        public Lexer(string source)
        {
            this.source = source;
            tokens = new List<Token>();
            startofLexeme = 0;
            currentPos = 0;
        }

        /// <summary>
        /// Scans the source code and generates a list of tokens.
        /// </summary>
        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                startofLexeme = currentPos;
                ScanToken();
            }
            tokens.Add(new Token(TokenType.EOF, "EOF", null));
            return tokens;
        }
        /// <summary>
        /// Scans a single token based on the current character.
        /// </summary>
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
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case ',': AddToken(TokenType.COMMA); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '-': AddToken(TokenType.MINUS); break;
                case '+': AddToken(TokenType.PLUS); break;
                case '*': AddToken(TokenType.MULTIPLY); break;
                case '/': AddToken(TokenType.DIVIDE); break;
                case '%': AddToken(TokenType.MODULUS); break;
                case '^': AddToken(TokenType.POWER); break;
                case '@': AddToken(TokenType.CONCAT); break;
                case '&': AddToken(TokenType.AND); break;
                case '|': AddToken(TokenType.OR); break;
                case '!': AddToken(Match('=') ? TokenType.NOT_EQUAL : TokenType.NOT); break;
                case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '=':
                    if (Match('>')) AddToken(TokenType.LAMBDA);
                    else AddToken(Match('=') ? TokenType.EQUAL : TokenType.ASSING); break;
                case '\"': ScanString(); break;
                //buscar numero, string o devolver error
                default:
                    if (char.IsDigit(c))
                    {
                        ScanNumber();
                        break;
                    }
                    if (char.IsLetter(c) || c == '_')
                    {
                        ScanIdentifier();
                        break;
                    }
                    throw new Error(ErrorType.LEXICAL, $"Character '{c}' is not supported.");
            }
        }
        /// <summary>
        /// Scans a number token.
        /// </summary>
        private void ScanNumber()
        {
            int dotCounter = 0;
            bool isvalidnumber = true;
            while (char.IsLetterOrDigit(Peek()) || Peek() == '.')
            {
                if (Peek() == '.') dotCounter++;
                if (char.IsLetter(Peek())) isvalidnumber = false;
                Advance();
            }
            if (dotCounter > 1 || !isvalidnumber)
                throw new Error(ErrorType.LEXICAL, $"Invalid token at '{GetLexeme()}'");
            else 
                AddToken(TokenType.NUMBER, double.Parse(GetLexeme()));
        }
        /// <summary>
        ///  Scans an identifier token.
        /// </summary>
        private void ScanIdentifier()
        {
            while (char.IsLetterOrDigit(Peek()) || Peek() == '_')
            {
                Advance();
            }
            string lexeme = GetLexeme();
            switch (lexeme)
            {
                case "let": AddToken(TokenType.LET, lexeme); break;
                case "in": AddToken(TokenType.IN, lexeme); break;
                case "if": AddToken(TokenType.IF, lexeme); break;
                case "else": AddToken(TokenType.ELSE, lexeme); break;
                case "print": AddToken(TokenType.PRINT, lexeme); break;
                case "function": AddToken(TokenType.FUNCTION, lexeme); break;
                case "true":
                case "false":
                    AddToken(TokenType.BOOLEAN, bool.Parse(lexeme)); break;
                case "PI": AddToken(TokenType.NUMBER, Math.PI); break;
                case "E": AddToken(TokenType.NUMBER, Math.E); break;
                default:
                    if (keywords.ContainsKey(lexeme.ToLower()))
                        throw new Error(ErrorType.LEXICAL, $"Invalid identifier at '{lexeme}'.");
                    else
                        AddToken(TokenType.IDENTIFIER, lexeme);
                    break;
            }
        }
        /// <summary>
        /// Scans a string token.
        /// </summary>
        private void ScanString()
        {
            while (Peek() != '\"')
            {
                if (IsAtEnd())
                {
                    throw new Error(ErrorType.LEXICAL, "Unfinished string.");
                }
                Advance();
            }
            Advance();
            //ignore ""
            string literal = GetLexeme();
            literal = literal.Substring(1, literal.Length - 2);
            AddToken(TokenType.STRING, literal);
        }
        /// <summary>
        /// Adds a token with the specified token type to the token list.
        /// </summary>
        private void AddToken(TokenType tokentype)
        {
            AddToken(tokentype, null);
        }
        /// <summary>
        /// Adds a token with the specified token type and literal value to the token list.
        /// </summary>
        private void AddToken(TokenType tokentype, Object literal)
        {
            string lexeme = GetLexeme();
            tokens.Add(new Token(tokentype, lexeme, literal));
        }
        /// <summary>
        /// Checks if the current character matches the expected character.
        /// </summary>
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (source[currentPos] != expected) return false;
            Advance();
            return true;
        }
        /// <summary>
        /// Returns the current character without advancing to the next character.
        /// </summary>
        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return source[currentPos];
        }
        /// <summary>
        /// Returns true if the lexer has reached the end of the source code.
        /// </summary>
        private bool IsAtEnd() 
        {
            return currentPos >= source.Length;
        }
        /// <summary>
        /// Returns the current character and advances to the next character.
        /// </summary>
        /// <returns></returns>
        private char Advance()
        {
            return source[currentPos++];
        }
        /// <summary>
        /// Retrieves the lexeme of the token from the source code.
        /// </summary>
        /// <returns></returns>
        private string GetLexeme()
        {
            return source.Substring(startofLexeme, currentPos - startofLexeme);
        }
    }
}