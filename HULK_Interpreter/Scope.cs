using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public class Scope
    {
        public Dictionary<string, object> Values { get; set; }
        private Scope? Parent;
        private Scope? Child;
        public Scope()
        {
            Values = new Dictionary<string, object>();
        }
        public Scope BuildChildScope()
        {
            Scope newScope = new Scope();
            Child = newScope;
            newScope.Parent = this;
            return newScope;
        }
        public object GetValue(string name)
        {
            if (Values.ContainsKey(name))
            {
                return Values;
            }
            else
            {
                if (Parent != null)
                {
                    return Parent.GetValue(name);
                }
                else
                {
                    throw new Error(ErrorType.SEMANTIC, $"Value of {name} wasn't declared.");
                }
            }
        }
        public void SetValue(string name, object value)
        {
            Values[name] = value;
        }
    }
}
