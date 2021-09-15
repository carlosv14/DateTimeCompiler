using System;
using DateTimeCompiler.Lexer.Tokens;

namespace DateTimeCompiler.Core.Expressions
{
    public class ConstantExpression: Expression
    {
        public string Lexeme { get; }

        public ConstantExpression(Token token, Type type, string lexeme) 
            : base(token, type)
        {
            Lexeme = lexeme;
        }

        public override dynamic Evaluate()
        {
            switch (Type.Lexeme)
            {
                case "Date":
                    return DateTime.Parse(Lexeme);
                case "TimeStamp":
                    return TimeSpan.Parse(Lexeme);
                case "Hour":
                    return int.Parse("10");
            }

            return null;
        }

        public override Type GetExpressionType()
        {
            return Type;
        }
    }
}