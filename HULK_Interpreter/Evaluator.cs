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
        private Dictionary<string, object> values;
        public Evaluator()
        {
            values = new Dictionary<string, object>();
        }
        public object Evaluate(Expression expression)
        {
            if (expression is LiteralExpression literal)
                return literal.Value;

            else if (expression is UnaryExpression unary)
                return EvaluateUnary(unary.Operator, Evaluate(unary.Right));

            else if (expression is BinaryExpression binary)
                return EvaluateBinary(Evaluate(binary.Left), binary.Operator, Evaluate(binary.Right));

            else if (expression is GroupingExpression grouping)
                return Evaluate(grouping.Expression);

            else if (expression is VariableExpression variable)
                return EvaluateVariable(variable.ID);
            
            else if (expression is PrintStatement printStatement)
                return Evaluate(printStatement.Expression);

            else if (expression is IfElseStatement ifelseStatement)
            {
                object condition = Evaluate(ifelseStatement.Condition);
                if (!IsBoolean(condition))
                    throw new Error(ErrorType.SEMANTIC, "Condition in 'If-Else' expression must be a boolean expression.");
                return (bool)condition ? Evaluate(ifelseStatement.ThenBranch) : Evaluate(ifelseStatement.ElseBranch);
            }

            else if (expression is LetInExpression letStatement)
            {
                foreach (AssignExpression assignment in letStatement.Assignments)
                {
                    values[assignment.ID.Lexeme] = Evaluate(assignment.Value);
                }
                return Evaluate(letStatement.Body);
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
                    if ((double)right != 0)
                        return (double)left / (double)right;
                    throw new Error(ErrorType.SEMANTIC, "Division by zero is undefined.");
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
                    return left.ToString() + right.ToString();

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
        public object EvaluateVariable(Token id)
        {
            string name = id.Lexeme;
            if (values.ContainsKey(name))
                return values[name];
            throw new Error(ErrorType.SEMANTIC, $"Value of '{name}' wasn't initialized.");
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
