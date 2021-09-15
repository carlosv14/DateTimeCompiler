using System;
using DateTimeCompiler.Core.Expressions;

namespace DateTimeCompiler.Core.Statements
{
    public class ExpressionStatement: Statement
    {
        public Expression Expression { get; }

        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }
        public override void ValidateSemantic()
        {
            //throw new System.NotImplementedException();
        }

        public override void Interpret()
        {
            Console.WriteLine($"{Expression.Evaluate()}");
        }
    }
}