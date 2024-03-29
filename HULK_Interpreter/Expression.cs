﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    /// <summary>
    /// Base class for all types of expressions.
    /// </summary>
    public abstract class Expression
    {
    }

    /// <summary>
    /// Represents a binary expression composed of a left expression, an operator token, and a right expression.
    /// </summary>
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

    /// <summary>
    /// Represents a unary expression composed of an operator token and a right expression.
    /// </summary>
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

    /// <summary>
    /// Represents a literal value expression, such as a number, boolean or string.
    /// </summary>
    public class LiteralExpression : Expression
    {
        public object Value { get; }

        public LiteralExpression(object value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Represents a grouping expression that wraps another expression.
    /// </summary>
    public class GroupingExpression : Expression
    {
        public Expression Expression { get; }

        public GroupingExpression(Expression expression)
        {
            Expression = expression;
        }
    }

    /// <summary>
    /// Represents an assignment expression, where a value is assigned to a variable identified by a token.
    /// </summary>
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

    /// <summary>
    /// Represents a variable expression identified by a token.
    /// </summary>
    public class VariableExpression : Expression
    {
        public Token ID { get; }

        public VariableExpression(Token id)
        {
            ID = id;
        }
    }

    /// <summary>
    /// Represents an if-else statement with a condition expression, a then branch expression, and an optional else branch expression.
    /// </summary>
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

    /// <summary>
    /// Represents a let-in expression, where a set of assignments are made followed by a body expression.
    /// </summary>
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

    /// <summary>
    /// Represents a function declaration with an identifier, a list of argument variable expressions, and a body expression.
    /// </summary>
    public class FunctionDeclaration : Expression
    {
        public string Identifier { get; }
        public List<VariableExpression> Arguments { get; }
        public Expression Body { get; }

        public FunctionDeclaration(string identifier, List<VariableExpression> arguments, Expression body)
        {
            Identifier = identifier;
            Arguments = arguments;
            Body = body;
        }
    }

    /// <summary>
    /// Represents a function call with an identifier and a list of argument expressions.
    /// </summary>
    public class FunctionCall : Expression
    {
        public string Identifier { get; }
        public List<Expression> Arguments { get; }

        public FunctionCall(string identifier, List<Expression> arguments)
        {
            Identifier = identifier;
            Arguments = arguments;
        }
    }
}
