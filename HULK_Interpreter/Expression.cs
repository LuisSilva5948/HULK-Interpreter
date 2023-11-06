using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public abstract class Expression
    {
    }
    public class BinaryExpression : Expression
    {
        public BinaryExpression(Expression left, Token op, Expression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        public Expression Left { get; }
        public Token Operator { get; }
        public Expression Right { get; }

        public object Evaluate(object left, object right)
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
    public class UnaryExpression : Expression
    {
        public UnaryExpression(Token op, Expression right)
        {
            Operator = op;
            Right = right;
        }
        public Token Operator { get; }
        public Expression Right { get; }

        public object Evaluate(object right)
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
    public class LiteralExpression : Expression
    {
        public object Value { get; }
        public LiteralExpression(object value)
        {
            Value = value;
        }
        public object Evaluate()
        {
            return Value;
        }
    }
    public class GroupingExpression : Expression
    {
        public Expression Expression { get; }
        public GroupingExpression(Expression expression)
        {
            Expression = expression;
        }
        public object Evaluate()
        {
            return Expression;
        }
    }
}
