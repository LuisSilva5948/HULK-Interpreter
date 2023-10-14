using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    internal class Error
    {
        private string message;

        private ErrorType errorType;

        public Error(ErrorType errorType, string message)
        {
            this.errorType = errorType;
            this.message = message;
        }
        public override string ToString()
        {
            return errorType + " error. " + message;
        }
    }
    enum ErrorType
    {
        Lexical,
        Syntax,
        Semantic
    }
}
