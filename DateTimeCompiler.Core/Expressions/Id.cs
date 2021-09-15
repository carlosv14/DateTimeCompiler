using DateTimeCompiler.Lexer.Tokens;

namespace DateTimeCompiler.Core.Expressions
{
    public class Id: Expression
    {
        public Id(Token token, Type type) 
            : base(token, type)
        {
        }

        public override dynamic Evaluate()
        {
            return EnvironmentManager.GetSymbolForEvaluation(Token.Lexeme).Value;
        }

        public override Type GetExpressionType()
        {
            return Type;
        }
    }
}