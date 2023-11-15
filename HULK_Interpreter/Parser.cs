using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HULK_Interpreter
{
    public class Parser
    {
        private readonly List<Token> tokens;
        private int currentPos;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            currentPos = 0;
        }
        public Expression Parse()
        {
            if (Match(TokenType.FUNCTION))
                return FunctionDeclaration();
            Expression AST = Expression();
            Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
            return AST;
        }
        
        private Expression Expression()
        {
            return Logical();
        }
        private Expression Logical()
        {
            Expression expression = Equality();
            while(Match(TokenType.AND, TokenType.OR))
            {
                Token Operator = Previous();
                Expression right = Equality();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        private Expression Equality()
        {
            Expression expression = Comparison();
            while(Match(TokenType.EQUAL, TokenType.NOT_EQUAL))
            {
                Token Operator = Previous();
                Expression right = Comparison();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        private Expression Comparison()
        {
            Expression expression = Concatenation();
            while (Match(TokenType.GREATER_EQUAL, TokenType.GREATER, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token Operator = Previous();
                Expression right = Concatenation();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        private Expression Concatenation()
        {
            Expression expression = Term();
            while (Match(TokenType.CONCAT))
            {
                Token Operator = Previous();
                Expression right = Concatenation();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        private Expression Term()
        {
            Expression expression = Factor();
            while (Match(TokenType.PLUS, TokenType.MINUS))
            {
                Token Operator = Previous();
                Expression right = Factor();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        private Expression Factor()
        {
            Expression expression = Power();
            while (Match(TokenType.MULTIPLY, TokenType.DIVIDE, TokenType.MODULUS))
            {
                Token Operator = Previous();
                Expression right = Power();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        private Expression Power()
        {
            Expression expression = Unary();
            if (Match(TokenType.POWER))
            {
                Token Operator = Previous();
                Expression right = Unary();
                return new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        private Expression Unary()
        {
            if(Match(TokenType.NOT, TokenType.MINUS))
            {
                Token Operator = Previous();
                Expression right = Unary();
                return new UnaryExpression(Operator, right);
            }
            return Literal();
        }
        private Expression Literal()
        {
            if (Match(TokenType.BOOLEAN, TokenType.NUMBER, TokenType.STRING))
            {
                return new LiteralExpression(Previous().Literal);
            }
            if (Match(TokenType.LEFT_PAREN))
            {
                Expression expression = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expected ')' after expression.");
                return new GroupingExpression(expression);
            }
            if (Match(TokenType.IDENTIFIER))
            {
                if (Memory.DeclaredFunctions.ContainsKey(Previous().Lexeme))
                    return FunctionCall();
                return new VariableExpression(Previous());
            }
            if (Match(TokenType.PRINT))
            {
                Consume(TokenType.LEFT_PAREN, "Expected '('.");
                Expression expression = PrintStatement();
                Consume(TokenType.RIGHT_PAREN, "Expected ')'.");
                return expression;
            }
            else if (Match(TokenType.IF))
            {
                Consume(TokenType.LEFT_PAREN, "Expected '('.");
                return IfElseExpression();
            }
            else if (Match(TokenType.LET))
            {
                return LetInExpression();
            }
            throw new Error(ErrorType.SYNTAX, "Invalid syntax.");
        }
        public Expression PrintStatement()
        {
            Expression statement = Expression();
            return new PrintStatement(statement);
        }
        public Expression IfElseExpression()
        {
            Expression condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expected ')'.");
            Expression thenBranch = Expression();
            Consume(TokenType.ELSE, "Expected 'else' at 'If-Else' expression.");
            Expression elseBranch = Expression();
            return new IfElseStatement(condition, thenBranch, elseBranch);
        }
        public Expression LetInExpression()
        {
            List<AssignExpression> assignments = new List<AssignExpression>();
            do
            {
                Token id = Consume(TokenType.IDENTIFIER, "Expected a variable name in a 'Let-In' expression.");
                Consume(TokenType.ASSING, $"Expected '=' when initializing variable '{id.Lexeme}'.");
                try
                {
                    Expression value = Expression();
                    assignments.Add(new AssignExpression(id, value));
                }
                catch (Error e)
                {
                    throw new Error(ErrorType.SYNTAX, $"Expected value of '{id.Lexeme}' after '='.");
                }
            }
            while (Match(TokenType.COMMA));

            Consume(TokenType.IN, "Expected 'in' at 'Let-In' expression.");
            Expression body = Expression();
            return new LetInExpression(assignments, body);
        }
        public Expression FunctionDeclaration()
        {
            string id = Consume(TokenType.IDENTIFIER, "Expected function name.").Lexeme;
            Consume(TokenType.LEFT_PAREN, "Expected '(' after function name.");
            List<string> parameters = new List<string>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    string param = Consume(TokenType.IDENTIFIER, "Expect parameter name.").Lexeme;
                    if (!parameters.Contains(param))
                        parameters.Add(param);
                    else throw new Error(ErrorType.SYNTAX, $"Parameter name '{param}' cannot be used more than once.");
                }
                while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after parameters.");
            Consume(TokenType.LAMBDA, "Missing '=>' operator in function declaration.");
            Expression body = Expression();

            if (Memory.DeclaredFunctions.ContainsKey(id))
                throw new Error(ErrorType.SYNTAX, $"Function '{id}' already exists and can't be redefined.");
            FunctionDeclaration function = new FunctionDeclaration(id, parameters, body);
            Memory.AddFunction(function);
            return function;
        }
        public Expression FunctionCall()
        {
            string id = Previous().Lexeme;
            FunctionDeclaration function = Memory.DeclaredFunctions[id];
            Consume(TokenType.LEFT_PAREN, $"Expected '(' after '{id}' call.");
            List<Expression> arguments = new List<Expression>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    arguments.Add(Expression());
                }
                while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_PAREN, $"Expected ')' after '{id}' arguments.");
            return new FunctionCall(id,arguments,function);
        }

        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }
        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }
        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }
        private Token Advance()
        {
            if (!IsAtEnd()) currentPos++;
            return Previous();
        }
        private Token Peek()
        {
            return tokens[currentPos];
        }
        private Token Previous()
        {
            return tokens[currentPos - 1];
        }
        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
                return Advance();

            throw new Error(ErrorType.SYNTAX, message);
        }
    }
}
