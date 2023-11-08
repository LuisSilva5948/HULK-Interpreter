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
                    CheckNumbers(Operator, left, right);
                    return (double)left + (double)right;
                case TokenType.MINUS:
                    CheckNumbers(Operator, left, right);
                    return (double)left - (double)right;
                case TokenType.MULTIPLY:
                    CheckNumbers(Operator, left, right);
                    return (double)left * (double)right;
                case TokenType.DIVIDE:
                    CheckNumbers(Operator, left, right);
                    return (double)left / (double)right;
                case TokenType.MODULUS:
                    CheckNumbers(Operator, left, right);
                    return (double)left % (double)right;
                case TokenType.POWER:
                    CheckNumbers(Operator, left, right);
                    return Math.Pow((double)left, (double)right);

                case TokenType.GREATER:
                    CheckNumbers(Operator, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    CheckNumbers(Operator, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    CheckNumbers(Operator, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    CheckNumbers(Operator, left, right);
                    return (double)left <= (double)right;

                case TokenType.EQUAL:
                case TokenType.NOT_EQUAL:
                    return IsEqual(left, right);

                case TokenType.AND:
                    CheckBooleans(Operator, left, right);
                    return (bool)left && (bool)right;
                case TokenType.OR:
                    CheckBooleans(Operator, left, right);
                    return (bool)left || (bool)right;

                case TokenType.CONCAT:
                    string leftstr = IsBoolean(left) ? left.ToString().ToLower() : left.ToString();
                    string rightstr = IsBoolean(right) ? right.ToString().ToLower() : right.ToString();
                    return leftstr + rightstr;

                default:
                    return null;
            }
        }
        public object EvaluateUnary(Token Operator, object right)
        {
            switch (Operator.Type)
            {
                case TokenType.NOT:
                    CheckBoolean(Operator, right);
                    return !(bool)right;
                case TokenType.MINUS:
                    CheckNumber(Operator, right);
                    return -(double)right;
                default:
                    return null;
            }
        }
        public void CheckBoolean(Token Operator, object right)
        {
            if (IsBoolean(right)) return;
            throw new Error(ErrorType.SEMANTIC, "Operand must be Boolean", Operator);
        }
        public void CheckBooleans(Token Operator, object left, object right)
        {
            if (IsBoolean(left, right)) return;
            throw new Error(ErrorType.SEMANTIC, "Operands must be Boolean", Operator);
        }
        public void CheckNumber(Token Operator, object right)
        {
            if (IsNumber(right)) return;
            throw new Error(ErrorType.SEMANTIC, "Operand must be Number", Operator);
        }
        public void CheckNumbers(Token Operator, object left, object right)
        {
            if (IsNumber(left, right)) return;
            throw new Error(ErrorType.SEMANTIC, "Operands must be Numbers", Operator);
        }
        public bool IsNumber(params object[] operands)
        {
            foreach (object operand in operands)
            {
                if (operand is not double)
                    return false;
            }
            return true;
        }
        public bool IsBoolean(params object[] operands)
        {
            foreach (object operand in operands)
            {
                if (operand is not bool)
                    return false;
            }
            return true;
        }
        public bool IsEqual(object left, object right)
        {
            if (left == null && right == null)
                return true;
            return left == null? false : left.Equals(right);
        }
    }
}
