using DateTimeCompiler.Lexer.Tokens;

namespace DateTimeCompiler.Core.Expressions
{
    public abstract class Expression
    {
        public Type Type { get; set; }
        public Token Token { get; set; }
        protected Expression(Token token, Type type)
        {
            Token = token;
            this.Type = type;
        }
        public abstract dynamic Evaluate();
        public abstract Type GetExpressionType();
    }
}