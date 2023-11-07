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
    }

    public class LiteralExpression : Expression
    {
        public object Value { get; }
        public LiteralExpression(object value)
        {
            Value = value;
        }
    }
    public class GroupingExpression : Expression
    {
        public Expression Expression { get; }
        public GroupingExpression(Expression expression)
        {
            Expression = expression;
        }
    }
}
