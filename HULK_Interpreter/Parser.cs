using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
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
            return Expression();
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
            throw new Error(ErrorType.SYNTAX, "Expression expected.", Peek());
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
