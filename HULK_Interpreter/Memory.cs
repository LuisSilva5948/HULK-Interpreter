using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public static class Memory
    {
        public static Dictionary<string, FunctionDeclaration> DeclaredFunctions { get; private set; }
        public static void Initialize()
        {
            DeclaredFunctions = new Dictionary<string, FunctionDeclaration>();
            DeclaredFunctions["sqrt"] = null;
            DeclaredFunctions["log"] = null;
            DeclaredFunctions["sen"] = null;
            DeclaredFunctions["cos"] = null;
        }
        public static void AddFunction(FunctionDeclaration function)
        {
            DeclaredFunctions[function.Identifier] = function;
        }
    }
}
