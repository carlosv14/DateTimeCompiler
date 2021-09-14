using System;
using System.Linq.Expressions;
using DateTimeCompiler.Lexer;
using DateTimeCompiler.Lexer.Tokens;

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
            Program();
        }

        private void Program()
        {
            States();
        }

        private void States()
        {
            State();
            InnerStates();
        }
        private void InnerStates()
        {
            if (this.lookAhead.TokenType == TokenType.VarKeyword
                || this.lookAhead.TokenType == TokenType.Variable
                || this.lookAhead.TokenType == TokenType.Number)
            {
                States();
            }
        }
        private void State()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.VarKeyword:
                    AssignationStatement();
                    break;
                default:
                    Expression();
                    break;
            }
        }

        private void AssignationStatement()
        {
            Match(TokenType.VarKeyword);
            Match(TokenType.Variable);
            Match(TokenType.Equal);
            Expression();
        }

        private void Expression()
        {
            Factor();
            while (this.lookAhead.TokenType == TokenType.Plus 
                   ||this.lookAhead.TokenType == TokenType.Substract )
            {
                Move();
                Factor();
            }
        }

        private void Factor()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Variable:
                    Match(TokenType.Variable);
                    break;
                case TokenType.Number:
                    Match(TokenType.Number);
                    if (this.lookAhead.TokenType==TokenType.Number)
                    {
                        Match(TokenType.Number);
                        if (this.lookAhead.TokenType == TokenType.Colon)
                        {
                            TimeStamp();
                        }
                        else if (this.lookAhead.TokenType == TokenType.Slash)
                        {
                            Date();
                        }
                        else
                        {
                            while (this.lookAhead.TokenType == TokenType.Number)
                            {
                                Match(TokenType.Number);
                            }
                        }
                    }
                    switch (this.lookAhead.TokenType)
                    {
                        case TokenType.HourKeyword:
                            Match(TokenType.HourKeyword);
                            break;
                        case TokenType.MinuteKeyword:
                            Match(TokenType.MinuteKeyword);
                            break;
                        case TokenType.SecondKeyword:
                            Match(TokenType.SecondKeyword);
                            break;
                        case TokenType.DayKeyword:
                            Match(TokenType.DayKeyword);
                            break;
                        case TokenType.MonthKeyword:
                            Match(TokenType.MonthKeyword);
                            break;
                        case TokenType.YearKeyword:
                            Match(TokenType.YearKeyword);
                            break;
                    }
                    break;
            }
        }

        private void Date()
        {
            Match(TokenType.Slash);
            Match(TokenType.Number);
            Match(TokenType.Number);
            Match(TokenType.Slash);
            Match(TokenType.Number);
            Match(TokenType.Number);
            Match(TokenType.Number);
            Match(TokenType.Number);
        }

        private void TimeStamp()
        {
            Match(TokenType.Colon);
            Match(TokenType.Number);
            Match(TokenType.Number);
            Match(TokenType.Colon);
            Match(TokenType.Number);
            Match(TokenType.Number);
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