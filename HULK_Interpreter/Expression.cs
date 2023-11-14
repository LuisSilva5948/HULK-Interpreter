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
    public class PrintStatement : Expression
    {
        public Expression Expression { get; }
        public PrintStatement(Expression expression)
        {
            Expression = expression;
        }
    }
    public class IfElseStatement : Expression
    {
        public Expression Condition { get; }
        public Expression ThenBranch { get; }
        public Expression ElseBranch { get; }

        public IfElseStatement(Expression condition, Expression thenBranch, Expression elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }
    }
    public class LetInExpression : Expression
    {
        public List<AssignExpression> Assignments { get; }
        public Expression Body { get; }

        public LetInExpression(List<AssignExpression> assignments, Expression body)
        {
            Assignments = assignments;
            Body = body;
        }
    }
    public class FunctionDeclaration : Expression
    {
        public Token Identifier { get; }
        public List<string> Parameters { get; }
        public Expression Body { get; }

        public FunctionDeclaration(Token identifier, List<string> parameters, Expression body)
        {
            Identifier = identifier;
            Parameters = parameters;
            Body = body;
        }
    }
    public class FunctionCall : Expression
    {
        public Token Identifier { get; }
        public List<Expression> Arguments { get; }
        public FunctionCall(Token identifier, List<Expression> arguments)
        {
            Identifier = identifier;
            Arguments = arguments;
        }
    }
}
