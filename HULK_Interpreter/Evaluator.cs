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
            /*switch (expression)
            {
                case BinaryExpression:
                    break;
                default:
                    break;
            }*/
            if (expression is BinaryExpression binary)
            {
                //return binary.Evaluate(EvaluateExpression(binary.Left), EvaluateExpression(binary.Right));
                return Binary(EvaluateExpression(binary.Left), binary.Operator, EvaluateExpression(binary.Right));
            }
            if (expression is LiteralExpression literal)
            {
                return literal.Evaluate();
            }
            return expression;
        }
        public object Binary(object left, Token Operator, object right)
        {
            switch (Operator.Type)
            {
                case TokenType.PLUS:
                    return (double)left + (double)right;
                case TokenType.MINUS:
                    return (double)left - (double)right;
                case TokenType.MULTIPLY:
                    return (double)left * (double)right;
                case TokenType.DIVIDE:
                    return (double)left / (double)right;
                case TokenType.MODULUS:
                    return (double)left % (double)right;
                default:
                    throw new Error(ErrorType.SEMANTIC, "Invalid binary operation.");
            }
        }
    }
}
