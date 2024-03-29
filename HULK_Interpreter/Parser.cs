﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HULK_Interpreter
{
    /// <summary>
    /// Represents a parser that analyzes the tokens produced by the lexer and constructs an abstract syntax tree (AST) for the source code.
    /// </summary>
    public class Parser
    {
        private readonly List<Token> Tokens;    // The list of tokens produced by the lexer
        private int CurrentPosition;            // The current position in the token list

        public Parser(List<Token> tokens)
        {
            Tokens = tokens;
            CurrentPosition = 0;
        }
        /// <summary>
        /// Parses the tokens and constructs an abstract syntax tree (AST).
        /// </summary>
        /// <returns>The abstract syntax tree.</returns>
        public Expression Parse()
        {
            if (Match(TokenType.FUNCTION))
                return FunctionDeclaration();
            Expression AST = Expression();
            Consume(TokenType.SEMICOLON, "Expected ';' after expression.");
            if (IsAtEnd())
                return AST;
            throw new Error(ErrorType.SYNTAX, "Invalid Syntax.");
        }
        /// <summary>
        /// Parses the global expression.
        /// </summary>
        private Expression Expression()
        {
            return Logical();
        }
        /// <summary>
        /// Parses a logical expression (&, |).
        /// </summary>
        private Expression Logical()
        {
            Expression expression = Equality();
            while (Match(TokenType.AND, TokenType.OR))
            {
                Token Operator = Previous();
                Expression right = Equality();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses an equality expression (==, !=).
        /// </summary>
        private Expression Equality()
        {
            Expression expression = Comparison();
            while (Match(TokenType.EQUAL, TokenType.NOT_EQUAL))
            {
                Token Operator = Previous();
                Expression right = Comparison();
                expression = new BinaryExpression(expression, Operator, right);
            }
            return expression;
        }
        /// <summary>
        /// Parses a comparison expression (>=, >, <, <=).
        /// </summary>
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
        /// <summary>
        /// Parses a concatenation expression (@).
        /// </summary>
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
        /// <summary>
        /// Parses a term expression (+, -).
        /// </summary>
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
        /// <summary>
        /// Parses a factor expression (*, /, %).
        /// </summary>
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
        /// <summary>
        /// Parses a power expression (^).
        /// </summary>
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
        /// <summary>
        /// Parses a unary expression (!, -).
        /// </summary>
        private Expression Unary()
        {
            if (Match(TokenType.NOT, TokenType.MINUS))
            {
                Token Operator = Previous();
                Expression right = Unary();
                return new UnaryExpression(Operator, right);
            }
            return Literal();
        }
        /// <summary>
        /// Parses a literal expression (number, string, boolean, group).
        /// </summary>
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
            return Primary();
        }
        /// <summary>
        /// Parses a primary expression (function call, variable, if-else, let-in).
        /// </summary>
        public Expression Primary()
        {
            if (Match(TokenType.IDENTIFIER))
            {
                if (Peek().Type == TokenType.LEFT_PAREN)
                    return FunctionCall();
                return new VariableExpression(Previous());
            }
            if (Match(TokenType.IF))
                return IfElseStatement();
            if (Match(TokenType.LET))
                return LetInExpression();
            throw new Error(ErrorType.SYNTAX, "Expected expression.");
        }
        /// <summary>
        /// Parses an if-else statement.
        /// </summary>
        public Expression IfElseStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expected '(' after 'if'.");
            Expression condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after 'if-else' condition.");
            Expression thenBranch = Expression();
            Consume(TokenType.ELSE, "Expected 'else' at 'if-else' expression.");
            Expression elseBranch = Expression();
            return new IfElseStatement(condition, thenBranch, elseBranch);
        }
        /// <summary>
        /// Parses a let-in expression.
        /// </summary>
        public Expression LetInExpression()
        {
            // Parse variable assignments
            List<AssignExpression> assignments = new List<AssignExpression>();
            do
            {
                Token id = Consume(TokenType.IDENTIFIER, "Expected a variable name in a 'let-in' expression.");
                Consume(TokenType.ASSIGN, $"Expected '=' when initializing variable '{id.Lexeme}'.");
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

            Consume(TokenType.IN, "Expected 'in' at 'let-in' expression.");
            Expression body = Expression();
            return new LetInExpression(assignments, body);
        }
        /// <summary>
        /// Parses a function declaration.
        /// </summary>
        public Expression FunctionDeclaration()
        {
            string id = Consume(TokenType.IDENTIFIER, "Expected function name.").Lexeme;
            // Check if function already exists
            if (Memory.DeclaredFunctions.ContainsKey(id))
                throw new Error(ErrorType.SYNTAX, $"Function '{id}' already exists and can't be redefined.");
            // Add function to declared functions temporarily
            Memory.DeclaredFunctions[id] = null;
            Consume(TokenType.LEFT_PAREN, "Expected '(' after function name.");
            // Parse function arguments
            List<VariableExpression> arguments = new List<VariableExpression>();
            if (!Check(TokenType.RIGHT_PAREN))
            {
                do
                {
                    Expression argument = Expression();

                    if (argument is not VariableExpression variable)
                        throw new Error(ErrorType.SYNTAX, "Expected valid variable name as an argument in function declaration.");
                    foreach (VariableExpression arg in arguments)
                    {
                        if (arg.ID == variable.ID)
                            throw new Error(ErrorType.SYNTAX, $"Parameter name '{variable.ID}' cannot be used more than once.");
                    }
                    arguments.Add(variable);
                }
                while (Match(TokenType.COMMA));
            }
            Consume(TokenType.RIGHT_PAREN, "Expected ')' after parameters.");
            Consume(TokenType.LAMBDA, "Missing '=>' operator in function declaration.");
            try
            {
                Expression body = Expression();
                FunctionDeclaration function = new FunctionDeclaration(id, arguments, body);
                // Add function to declared functions permanently
                Memory.AddFunction(function);
                return function;
            }
            catch (Error e)
            {
                Memory.DeclaredFunctions.Remove(id);
                throw new Error(ErrorType.SYNTAX, $"Invalid declaration of function '{id}'.");
            }
        }
        /// <summary>
        /// Parses a function call.
        /// </summary>
        public Expression FunctionCall()
        {
            string id = Previous().Lexeme;
            Consume(TokenType.LEFT_PAREN, $"Expected '(' after '{id}' call.");
            // Parse function arguments
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
            return new FunctionCall(id, arguments);
        }

        #region Helper Methods
        /// <summary>
        /// Matches the given token types and advances the current position if a match is found.
        /// </summary>
        /// <param name="types">The token types to match.</param>
        /// <returns>True if a match is found; otherwise, false.</returns>
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

        /// <summary>
        /// Checks if the current token has the specified type.
        /// </summary>
        /// <param name="type">The token type to check.</param>
        /// <returns>True if the current token has the specified type; otherwise, false.</returns>
        private bool Check(TokenType type)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == type;
        }

        /// <summary>
        /// Checks if the current position is at the end of the token list.
        /// </summary>
        /// <returns>True if the current position is at the end; otherwise, false.</returns>
        private bool IsAtEnd()
        {
            return Peek().Type == TokenType.EOF;
        }

        /// <summary>
        /// Advances the current position to the next token and returns the previous token.
        /// </summary>
        /// <returns>The previous token.</returns>
        private Token Advance()
        {
            if (!IsAtEnd()) CurrentPosition++;
            return Previous();
        }

        /// <summary>
        /// Returns the token at the current position without advancing the position.
        /// </summary>
        /// <returns>The token at the current position.</returns>
        private Token Peek()
        {
            return Tokens[CurrentPosition];
        }

        /// <summary>
        /// Returns the previous token.
        /// </summary>
        /// <returns>The previous token.</returns>
        private Token Previous()
        {
            return Tokens[CurrentPosition - 1];
        }

        /// <summary>
        /// Consumes the current token if it has the specified type; otherwise, throws a syntax error.
        /// </summary>
        /// <param name="type">The expected token type.</param>
        /// <param name="message">The error message to throw.</param>
        /// <returns>The consumed token.</returns>
        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
                return Advance();

            throw new Error(ErrorType.SYNTAX, message);
        }
        #endregion
    }
}
