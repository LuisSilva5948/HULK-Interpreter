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
        public Token Variable { get; }
        public Expression Initializer { get; }
        public Statement Body { get; }

        public LetStatement(Token variable, Expression initializer, Statement body)
        {
            Variable = variable;
            Initializer = initializer;
            Body = body;
        }

        /*public override object Evaluate()
        {
            // Crear un nuevo entorno (scope) para la variable
            Environment environment = new Environment();

            // Evaluar la expresión inicializadora y asignar el valor a la variable en el entorno
            object value = Initializer.Evaluate();
            environment.DefineVariable(Variable.Lexeme, value);

            // Evaluar el cuerpo del let statement en el nuevo entorno
            return Body.Evaluate(environment);
        }*/
    }
}
