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
        public Error(ErrorType errorType, string message)
        {
            this.errorType = errorType;
            this.message = message;
        }
        public override string ToString()
        {
            return $"! {errorType} ERROR. {message}";
        }
    }
    public enum ErrorType
    {
        LEXICAL,
        SYNTAX,
        SEMANTIC
    }
}