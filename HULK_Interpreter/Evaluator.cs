using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
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
        public object Evaluate(Expression expression)
        {
            if (expression is LiteralExpression literal)
            {
                return literal.Value;
            }
            else if (expression is UnaryExpression unary)
            {
                return EvaluateUnary(unary.Operator, Evaluate(unary.Right));
            }
            else if (expression is BinaryExpression binary)
            {
                return EvaluateBinary(Evaluate(binary.Left), binary.Operator, Evaluate(binary.Right));
            }
            else if (expression is GroupingExpression grouping)
            {
                return Evaluate(grouping.Expression);
            }
            else return null;
        }

        public object EvaluateBinary(object left, Token Operator, object right)
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
                case TokenType.POWER:
                    return Math.Pow((double)left, (double)right);

                case TokenType.EQUAL:
                    return (double)left == (double)right;
                case TokenType.GREATER:
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    return (double)left <= (double)right;

                case TokenType.AND:
                    return (bool)left && (bool)right;
                case TokenType.OR:
                    return (bool)left || (bool)right;

                case TokenType.CONCAT:
                    return left.ToString() + right.ToString();
                default:
                    throw new Error(ErrorType.SEMANTIC, "Invalid binary operation.");
            }
        }
        public object EvaluateUnary(Token Operator, object right)
        {
            switch (Operator.Type)
            {
                case TokenType.NOT:
                    return !(bool)right;
                case TokenType.MINUS:
                    return -(double)right;
                default:
                    throw new Error(ErrorType.SEMANTIC, "Invalid unary operation.");
            }
        }
    }
}
