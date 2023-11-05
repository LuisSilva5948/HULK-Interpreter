using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    internal class Evaluator
    {
        private Expression AST;
        public Evaluator(Expression AST)
        {
            this.AST = AST;
        }
        public object EvaluateExpression(Expression expression)
        {
            switch (expression)
            {
                case BinaryExpression:
                    break;
                default:
                    break;
            }
            if (expression is BinaryExpression binary)
            {
                return binary.Evaluate(EvaluateExpression(binary.Left), EvaluateExpression(binary.Right));
            }
            if (expression is LiteralExpression literal)
            {
                return literal.Evaluate();
            }
            return expression;
        }
    }
}
