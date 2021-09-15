using System;
using DateTimeCompiler.Core.Expressions;

namespace DateTimeCompiler.Core.Statements
{
    public class AssignationStatement: Statement
    {
        public Id Id { get; }
        public Expression Expression { get; }

        public AssignationStatement(Id id, Expression expression)
        {
            Id = id;
            Expression = expression;
        }
        public override void ValidateSemantic()
        {
            if (Id.GetExpressionType() != Expression.GetExpressionType())
            {
                throw new ApplicationException($"Types different");
            }
        }

        public override void Interpret()
        {
            EnvironmentManager.UpdateVariable(Id.Token.Lexeme,Expression.Evaluate());
        }
    }
}