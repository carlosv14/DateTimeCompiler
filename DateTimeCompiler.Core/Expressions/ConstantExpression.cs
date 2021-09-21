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
                case "Minute":
                case "Second":
                case "Year":
                case "Month":
                case "Day":
                    return int.Parse(Lexeme);
            }

            return null;
        }

        public override Type GetExpressionType()
        {
            return Type;
        }
    }
}