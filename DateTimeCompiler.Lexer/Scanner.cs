using System;
using System.Collections.Generic;
using System.Text;
using DateTimeCompiler.Lexer.Tokens;

namespace DateTimeCompiler.Lexer
{
    public class Scanner
    {
        private Input input;
        private readonly Dictionary<string, TokenType> keywords;

        public Scanner(Input input)
        {
            this.input = input;
            this.keywords = new Dictionary<string, TokenType>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"var", TokenType.VarKeyword},
                {"Day", TokenType.DayKeyword},
                {"Month", TokenType.MonthKeyword},
                {"Year", TokenType.YearKeyword},
                {"Hour", TokenType.HourKeyword},
                {"Minute", TokenType.MinuteKeyword},
                {"Second", TokenType.SecondKeyword}
            };
        }

        public Token GetNextToken()
        {
            var currentChar = GetNextChar();
            StringBuilder lexeme = new StringBuilder();

            while (true)
            {
                while (char.IsWhiteSpace(currentChar) 
                || currentChar.Equals('\n'))
                {
                    currentChar = GetNextChar();
                }

                if (char.IsDigit(currentChar))
                {
                    lexeme.Append(currentChar);
                    return new Token
                    {
                        TokenType = TokenType.Number,
                        Column = input.Position.Column,
                        Lexeme = lexeme.ToString(),
                        Line = input.Position.Line
                    };
                }
                else if (char.IsLetter(currentChar))
                {
                    lexeme.Append(currentChar);
                    currentChar = GetNextChar();
                    while (char.IsLetter(currentChar))
                    {
                        lexeme.Append(currentChar);
                        currentChar = GetNextChar();
                    }

                    if (!keywords.ContainsKey(lexeme.ToString()))
                    {
                        return new Token
                        {
                            TokenType = TokenType.Variable,
                            Column = input.Position.Column,
                            Lexeme = lexeme.ToString(),
                            Line = input.Position.Line
                        };
                    }
                    else
                    {
                        return new Token
                        {
                            TokenType = keywords[lexeme.ToString()],
                            Column = input.Position.Column,
                            Lexeme = lexeme.ToString(),
                            Line = input.Position.Line
                        };
                    }
                }
                else 
                {
                    switch (currentChar)
                    {
                        case '+':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Plus,
                                Column = input.Position.Column,
                                Lexeme = lexeme.ToString(),
                                Line = input.Position.Line
                            };
                        case '-':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Substract,
                                Column = input.Position.Column,
                                Lexeme = lexeme.ToString(),
                                Line = input.Position.Line
                            };
                        case '=':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Equal,
                                Column = input.Position.Column,
                                Lexeme = lexeme.ToString(),
                                Line = input.Position.Line
                            };
                        case '/':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Slash,
                                Column = input.Position.Column,
                                Lexeme = lexeme.ToString(),
                                Line = input.Position.Line
                            };
                        case ':':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.Colon,
                                Column = input.Position.Column,
                                Lexeme = lexeme.ToString(),
                                Line = input.Position.Line
                            };
                        case '\0':
                            lexeme.Append(currentChar);
                            return new Token
                            {
                                TokenType = TokenType.EOF,
                                Column = input.Position.Column,
                                Lexeme = lexeme.ToString(),
                                Line = input.Position.Line
                            };
                        default:
                            throw new ApplicationException(
                                $"Character is invalid in Lexeme: {lexeme} Column: {input.Position.Column} and Line: {input.Position.Line}");
                    }
                }
            }
        }

        private char GetNextChar()
        {
            var next = input.NextChar();
            input = next.Reminder;
            return next.Value;
        }

        private char PeekNextChar()
        {
            var next = input.NextChar();
            return next.Value;
        }
    }
}