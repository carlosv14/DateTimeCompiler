using System;
using System.Collections.Generic;
using System.Linq;
using DateTimeCompiler.Core;
using DateTimeCompiler.Core.Expressions;
using DateTimeCompiler.Core.Statements;
using DateTimeCompiler.Lexer;
using DateTimeCompiler.Lexer.Tokens;
using Type = DateTimeCompiler.Core.Type;

// ReSharper disable once InvalidXmlDocComment
/// <summary>
/// Codigo -> List<Statements>
/// List<Statements> -> Statement List<Statements>
/// Statement -> AssignationStatement | Expression
/// AssignationStatement -> Var Id = Expression
/// Expression -> E + F | E - F
/// F -> Id | TimeStamp | Hour | Day | Month | Year | Date | Minute | Second
/// TimeStamp -> Digit Digit : Digit Digit : Digit Digit
/// Hour -> [(Digit)+]Hour
/// Minute ->[(Digit)+]Minute
/// Second ->[(Digit)+]Second
/// Day ->[(Digit)+]Day
/// Month ->[(Digit)+]Month
/// Year ->[(Digit)+]Year
/// Date -> Digit Digit / Digit Digit / Digit Digit Digit Digit
/// </summary>

namespace DateTimeCompiler.Parser
{
    public class Parser
    {
        private readonly Scanner scanner;
        private Token lookAhead;

        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
            this.Move();
        }
        public void Parse()
        {
            var program =Program();
            program.ValidateSemantic();
            program.Interpret();
        }

        private Statement Program()
        {
            EnvironmentManager.PushContext();
            return States();
        }

        private Statement States()
        {
            return new SequenceStatement(State(), InnerStates());
        }
        private Statement InnerStates()
        {
            if (this.lookAhead.TokenType == TokenType.VarKeyword
                || this.lookAhead.TokenType == TokenType.Variable
                || this.lookAhead.TokenType == TokenType.Number)
            {
                return States();
            }

            return null;
        }
        private Statement State()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.VarKeyword:
                    return AssignationStatement();
                default:
                    return new ExpressionStatement(Expression());
            }
        }

        private Statement AssignationStatement()
        {
            Match(TokenType.VarKeyword);
            var token = lookAhead;
            Match(TokenType.Variable);
            var id = new Id(token, Type.Date);
            EnvironmentManager.AddVariable(token.Lexeme,id);
            Match(TokenType.Equal);
            return new AssignationStatement(id,Expression());
        }

        private Expression Expression()
        {
            var factors = Factor();
            while (this.lookAhead.TokenType == TokenType.Plus 
                   ||this.lookAhead.TokenType == TokenType.Substract )
            {
                var token = lookAhead;
                Move();
                factors = new BinaryExpression(token,factors,Factor());
            }

            return factors;
        }

        private Expression Factor()
        {
            List<Token> varList = new List<Token>();
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Variable:
                    var token = lookAhead;
                    Match(TokenType.Variable);
                    return EnvironmentManager.GetSymbol(token.Lexeme).Id;
                default:
                    token = lookAhead;
                    Match(TokenType.Number);
                    if (this.lookAhead.TokenType==TokenType.Number)
                    {
                        var token2 = lookAhead;
                        Match(TokenType.Number);
                        if (this.lookAhead.TokenType == TokenType.Colon)
                        {
                            return TimeStamp(token, token2);
                        }
                        else if (this.lookAhead.TokenType == TokenType.Slash)
                        {
                            return Date(token, token2);
                        }
                        else
                        {
                            while (this.lookAhead.TokenType == TokenType.Number)
                            {
                                varList.Add(lookAhead);
                                Match(TokenType.Number);
                            }
                        }
                    }

                    ConstantExpression constantExpression;
                    switch (this.lookAhead.TokenType)
                    {
                        case TokenType.HourKeyword:
                            token = lookAhead;
                            Match(TokenType.HourKeyword);
                            constantExpression = new ConstantExpression(token,Type.Hour, string.Concat(varList.Select(x => x.Lexeme)));
                            break;
                        case TokenType.MinuteKeyword:
                            token = lookAhead;
                            Match(TokenType.MinuteKeyword);
                            constantExpression = new ConstantExpression(token, Type.Minute, string.Concat(varList.Select(x => x.Lexeme)));
                            break;
                        case TokenType.SecondKeyword:
                            token = lookAhead;
                            Match(TokenType.SecondKeyword);
                            constantExpression = new ConstantExpression(token, Type.Second, string.Concat(varList.Select(x => x.Lexeme)));
                            break;
                        case TokenType.DayKeyword:
                            token = lookAhead;
                            Match(TokenType.DayKeyword);
                            constantExpression = new ConstantExpression(token, Type.Day, string.Concat(varList.Select(x => x.Lexeme)));
                            break;
                        case TokenType.MonthKeyword:
                            token = lookAhead;
                            Match(TokenType.MonthKeyword);
                            constantExpression = new ConstantExpression(token, Type.Month, string.Concat(varList.Select(x => x.Lexeme)));
                            break;
                        default:
                            token = lookAhead;
                            Match(TokenType.YearKeyword);
                            constantExpression = new ConstantExpression(token, Type.Year, string.Concat(varList.Select(x => x.Lexeme)));
                            break;
                    }
                    return constantExpression;
            }
        }

        private Expression Date(Token token1, Token token2)
        {
            List<Token> list = new List<Token>();
            list.Add(token1);
            list.Add(token2);
            list.Add(lookAhead);
            Match(TokenType.Slash);
            list.Add(lookAhead);
            Match(TokenType.Number);
            list.Add(lookAhead);
            Match(TokenType.Number);
            list.Add(lookAhead);
            Match(TokenType.Slash);
            list.Add(lookAhead);
            Match(TokenType.Number);
            list.Add(lookAhead);
            Match(TokenType.Number);
            list.Add(lookAhead);
            Match(TokenType.Number);
            list.Add(lookAhead);
            Match(TokenType.Number);
            return new ConstantExpression(null , Type.Date, string.Concat(list.Select(x => x.Lexeme)));
        }

        private Expression TimeStamp(Token token1, Token token2)
        {
            List<Token> list = new List<Token>();
            list.Add(token1);
            list.Add(token2);
            list.Add(lookAhead);
            Match(TokenType.Colon);
            list.Add(lookAhead);
            Match(TokenType.Number);
            list.Add(lookAhead);
            Match(TokenType.Number);
            list.Add(lookAhead);
            Match(TokenType.Colon);
            list.Add(lookAhead);
            Match(TokenType.Number);
            list.Add(lookAhead);
            Match(TokenType.Number);
            return new ConstantExpression(null, Type.TimeStamp, string.Concat(list.Select(x => x.Lexeme)));
        }

        private void Move()
        {
            this.lookAhead = this.scanner.GetNextToken();
        }

        private void Match(TokenType tokenType)
        {
            if (this.lookAhead.TokenType != tokenType)
            {
                throw new ApplicationException($"Syntax error! expected token {tokenType} but found {this.lookAhead.TokenType}. Line: {this.lookAhead.Line}, Column: {this.lookAhead.Column}");
            }
            this.Move();
        }
    }
}