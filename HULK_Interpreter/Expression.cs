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
        public BinaryExpression(Expression left, Token Operator, Expression right)
        {
            this.Left = left;
            this.Operator = Operator;
            this.Right = right;
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
                default:
                    throw new Error(ErrorType.SEMANTIC, "Invalid operation.");
            }
        }
    }
    public class UnaryExpression : Expression
    {
        public UnaryExpression(Token Operator, Expression right)
        {
            this.Operator = Operator;
            this.Right = right;
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
                    throw new Error(ErrorType.SEMANTIC, "Invalid operation.");
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
        public Expression expression { get; }
        public GroupingExpression(Expression expression)
        {
            this.expression = expression;
        }
        public object Evaluate()
        {
            return expression;
        }
    }
}
