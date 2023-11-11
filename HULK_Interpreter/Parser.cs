﻿using System;
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
            Expression expression = Expression();
            Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
            return expression;
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
                //if is not an call to a function
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
            throw new Error(ErrorType.SYNTAX, "Expression expected.");
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
            Consume(TokenType.ELSE, "Expected 'else' at If-Else statement.");
            Expression elseBranch = Expression();
            return new IfElseStatement(condition, thenBranch, elseBranch);
        }
        public Expression LetInExpression()
        {
            List<AssignExpression> assignments = new List<AssignExpression>();
            do
            {
                Token id = Consume(TokenType.IDENTIFIER, "Expected a variable name.");
                Consume(TokenType.ASSING, "Expected '='.");
                Expression value = Expression();
                assignments.Add(new AssignExpression(id, value));
            }
            while (Match(TokenType.COMMA));

            Consume(TokenType.IN, "Expected 'in' at Let-In expression.");
            Expression body = Expression();
            return new LetInExpression(assignments, body);
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
