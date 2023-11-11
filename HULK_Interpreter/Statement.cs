using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    public abstract class Statement
    {
    }
    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; }
        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }
    }
    public class PrintStatement: Statement
    {
        public Statement Statement { get; }
        public PrintStatement(Statement statement)
        {
            Statement = statement;
        }
    }
    public class IfStatement : Statement
    {
        public Expression Condition { get; }
        public Statement ThenBranch { get; }
        public Statement ElseBranch { get; }

        public IfStatement(Expression condition, Statement thenBranch, Statement elseBranch)
        {
            Condition = condition;
            ThenBranch = thenBranch;
            ElseBranch = elseBranch;
        }
    }
    public class LetStatement : Statement
    {
        public List<AssignExpression> Assignments { get; }
        public Statement Body { get; }

        public LetStatement(List<AssignExpression> assignments, Statement body)
        {
            Assignments = assignments;
            Body = body;
        }
    }
}
