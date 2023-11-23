using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace HULK_Interpreter
{
    internal class Evaluator
    {
        private Stack<Dictionary<string, object>> Scopes;
        private readonly int CallLimit = 100000;
        private int Calls;

        public Evaluator()
        {
            Scopes = new Stack<Dictionary<string, object>>();
            Scopes.Push(new Dictionary<string, object>());
        }

        private void PushScope()
        {
            Dictionary<string, object> newScope = new Dictionary<string, object>();
            foreach (var keyvaluepair in CurrentScope())
                newScope[keyvaluepair.Key] = keyvaluepair.Value;
            Scopes.Push(newScope);
        }

        private void PopScope()
        {
            Scopes.Pop();
        }

        private Dictionary<string, object> CurrentScope()
        {
            return this.Scopes.Peek();
        }
        
        /*private Scope Scope;
        readonly int CallLimit = 1000;
        private int Calls;
        public Evaluator()
        {
            Scope = new Scope();
            Calls = 0;
        }*/
        public object Evaluate(Expression expression)
        {
            if (Calls > CallLimit)
                throw new Error(ErrorType.SEMANTIC, "Stack Overflow.");
            Calls++;

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
                return EvaluateLetIn(letStatement);
            }

            else if (expression is FunctionDeclaration function)
            {
                return $"Function '{function.Identifier}' was declared succesfully.";
            }

            else if (expression is FunctionCall call)
            {
                return EvaluateFunction(call);
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
            return CurrentScope().ContainsKey(name)? CurrentScope()[name] : throw new Error(ErrorType.SEMANTIC, $"Value of {name} wasn't declared.");
        }
        public object EvaluateLetIn(LetInExpression letIn)
        {
            PushScope();
            foreach (AssignExpression assign in letIn.Assignments)
            {
                object value = Evaluate(assign.Value);
                CurrentScope()[assign.ID.Lexeme] = value;
            }
            object result = Evaluate(letIn.Body);
            PopScope();
            return result;
        }
        public object EvaluateFunction(FunctionCall call)
        {
            //evaluate the arguments
            List<object> args = new List<object>();
            foreach (Expression arg in call.Arguments)
            {
                args.Add(Evaluate(arg));
            }
            switch (call.Identifier)
            {
                case "print":
                    if (args.Count != 1)
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' receives '{args.Count}' argument(s) instead of the correct amount '1'");
                    return args[0];
                case "sen":
                    if (args.Count != 1)
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' receives '{args.Count}' argument(s) instead of the correct amount '1'");
                    if (!IsNumber(args[0]))
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' can only receives 'Number'.");
                    return Math.Sin((double)args[0]);
                case "cos":
                    if (args.Count != 1)
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' receives '{args.Count}' argument(s) instead of the correct amount '1'"); ;
                    if (!IsNumber(args[0]))
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' can only receives 'Number'.");
                    return Math.Cos((double)args[0]);
                case "sqrt":
                    if (args.Count != 1)
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' receives '{args.Count}' argument(s) instead of the correct amount '1'");
                    if (!IsNumber(args[0]))
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' can only receives 'Number'.");
                    return Math.Sqrt((double)args[0]);
                case "log":
                    if (args.Count != 2)
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' receives '{args.Count}' argument(s) instead of the correct amount '2'");
                    if (!IsNumber(args[0], args[1]))
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' can only receives 'Number'.");
                    return Math.Log((double)args[0], (double)args[1]);
                case "exp":
                    if (args.Count != 1)
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' receives '{args.Count}' argument(s) instead of the correct amount '1'");
                    if (!IsNumber(args[0]))
                        throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' can only receives 'Number'.");
                    return Math.Exp((double)args[0]);
                default:
                    //functions declared by the user
                    if (Memory.DeclaredFunctions.ContainsKey(call.Identifier))
                    {
                        FunctionDeclaration function = Memory.DeclaredFunctions[call.Identifier];
                        //check the amount of arguments of the function vs the arguments passed
                        if (args.Count != function.Arguments.Count)
                            throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' receives '{args.Count}' argument(s) instead of the correct amount '{function.Arguments.Count}'");
                        PushScope();
                        //bind the argument values to the parameter names in the new scope
                        for (int i = 0; i < function.Arguments.Count; i++)
                        {
                            string parameterName = function.Arguments[i].ID.Lexeme;
                            object argumentValue = args[i];
                            CurrentScope()[parameterName] = argumentValue;
                        }
                        // Evaluate the body of the function
                        object result = Evaluate(function.Body);
                        PopScope();
                        return result;
                    }
                    else throw new Error(ErrorType.SEMANTIC, $"Function '{call.Identifier}' wasn't declared.");
            }
        }
        //checkers
        public void CheckBoolean(Token Operator, object right)
        {
            if (IsBoolean(right)) return;
            throw new Error(ErrorType.SEMANTIC, $"Operand must be Boolean in '{Operator.Lexeme}' operation.");
        }
        public void CheckBooleans(Token Operator, object left, object right)
        {
            if (IsBoolean(left, right)) return;
            throw new Error(ErrorType.SEMANTIC, $"Operands must be Boolean in '{Operator.Lexeme}' operation.");
        }
        public void CheckNumber(Token Operator, object right)
        {
            if (IsNumber(right)) return;
            throw new Error(ErrorType.SEMANTIC, $"Operand must be Number in '{Operator.Lexeme}' operation.");
        }
        public void CheckNumbers(Token Operator, object left, object right)
        {
            if (IsNumber(left, right)) return;
            throw new Error(ErrorType.SEMANTIC, $"Operands must be Numbers in '{Operator.Lexeme}' operation.");
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