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
        public BinaryExpression(Expression left, Token _operator, Expression right)
        {
            this.Left = left;
            this.Operator = _operator;
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
                    throw new Exception("Invalid operation.");
            }
        }
    }

    
    // Resto de las clases de expresiones...



    /*public abstract class Expression
    {
        public abstract object Evaluate();
    }

    public class EqualityExpression : Expression
    {
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }
        public string Operator { get; private set; }

        public EqualityExpression(Expression left, Expression right, string _operator)
        {
            Left = left;
            Right = right;
            Operator = _operator;
        }

        public override object Evaluate()
        {
            var left = Left.Evaluate();
            var right = Right.Evaluate();

            switch (Operator)
            {
                case "==":
                    return left == right;
                case "!=":
                    return left != right;
                default:
                    throw new Exception($"Invalid operator: {Operator}");
            }
        }
    }

    public class ComparisonExpression : Expression
    {
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }
        public string Operator { get; private set; }

        public ComparisonExpression(Expression left, Expression right, string @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        public override object Evaluate()
        {
            var left = Left.Evaluate();
            var right = Right.Evaluate();

            switch (Operator)
            {
                case ">":
                    return (double)left > (double)right;
                case ">=":
                    return (double)left >= (double)right;
                case "<":
                    return (double)left < (double)right;
                case "<=":
                    return (double)left <= (double)right;
                default:
                    throw new Exception($"Invalid operator: {Operator}");
            }
        }
    }

    public class TermExpression : Expression
    {
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }
        public string Operator { get; private set; }

        public TermExpression(Expression left, Expression right, string @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        public override object Evaluate()
        {
            var left = (double)Left.Evaluate();
            var right = (double)Right.Evaluate();

            switch (Operator)
            {
                case "+":
                    return left + right;
                case "-":
                    return left - right;
                default:
                    throw new Exception($"Invalid operator: {Operator}");
            }
        }
    }

    public class FactorExpression : Expression
    {
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }
        public string Operator { get; private set; }

        public FactorExpression(Expression left, Expression right, string @operator)
        {
            Left = left;
            Right = right;
            Operator = @operator;
        }

        public override object Evaluate()
        {
            var left = (double)Left.Evaluate();
            var right = (double)Right.Evaluate();

            switch (Operator)
            {
                case "*":
                    return left * right;
                case "/":
                    return left / right;
                default:
                    throw new Exception($"Invalid operator: {Operator}");
            }
        }
    }

    public class UnaryExpression : Expression
    {
        public Expression Operand { get; private set; }
        public string Operator { get; private set; }

        public UnaryExpression(Expression operand, string _operator)
        {
            Operand = operand;
            Operator = _operator;
        }

        public override object Evaluate()
        {
            var operandValue = (double)Operand.Evaluate();

            switch (Operator)
            {
                case "-":
                    return -operandValue;
                case "!":
                    return !Convert.ToBoolean(operandValue);
                default:
                    throw new Exception($"Invalid operator: {Operator}");
            }
        }
    }

    public class PrimaryExpression : Expression
    {
        public object Value { get; private set; }

        public PrimaryExpression(object value)
        {
            Value = value;
        }

        public override object Evaluate()
        {
            return Value;
        }
    }*/
}
