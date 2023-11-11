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
        public Expression Left { get; }
        public Token Operator { get; }
        public Expression Right { get; }
        public BinaryExpression(Expression left, Token op, Expression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }

    public class UnaryExpression : Expression
    {
        public Token Operator { get; }
        public Expression Right { get; }
        public UnaryExpression(Token op, Expression right)
        {
            Operator = op;
            Right = right;
        }
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

    public class AssignExpression : Expression
    {
        public Token ID { get; }
        public Expression Value { get; }
        public AssignExpression(Token id, Expression value)
        {
            ID = id;
            Value = value;
        }
    }
    public class VariableExpression : Expression
    {
        public Token ID { get; }
        public VariableExpression(Token id)
        {
            ID = id;
        }
    }
}
