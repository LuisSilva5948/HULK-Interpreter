using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public class Error : Exception
    {
        public string message { get; private set; }
        public ErrorType errorType { get; private set; }
        public Token token { get; private set; }
        public Error(ErrorType errorType, string message): base(message)
        {
            this.errorType = errorType;
            this.message = message;
        }
        public Error(ErrorType errorType, string message, Token token) : base(message)
        {
            this.errorType = errorType;
            this.message = message;
            this.token = token;
        }
        public string Report()
        {
            if (token == null)
                return $"! {errorType} ERROR: {message}";
            else
                return $"! {errorType} ERROR: {message} at {token.Lexeme}";
        }
    }
    public enum ErrorType
    {
        LEXICAL,
        SYNTAX,
        SEMANTIC
    }
}