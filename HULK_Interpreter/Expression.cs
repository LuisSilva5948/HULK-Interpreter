using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public abstract class Expression
    {
        public abstract object Evaluate();
    }

    public class AssignExpr : Expression
    {
        private Token name;
        private Expression value;

        public AssignExpr(Token name, Expression value)
        {
            this.name = name;
            this.value = value;
        }

        public override object Evaluate()
        {
            // Implementar la lógica para evaluar una expresión de asignación
            // y devolver el resultado.
            return value;
        }
    }

    public class BinaryExpr : Expression
    {
        private Expression left;
        private Token operator_;
        private Expression right;

        public BinaryExpr(Expression left, Token operator_, Expression right)
        {
            this.left = left;
            this.operator_ = operator_;
            this.right = right;
        }

        public override object Evaluate()
        {
            // Implementar la lógica para evaluar una expresión binaria
            // y devolver el resultado.
            return left.Evaluate();
        }
    }

    // Otras subclases para los demás tipos de expresión...


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
