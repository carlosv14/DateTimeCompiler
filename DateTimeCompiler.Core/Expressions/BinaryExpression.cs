using System;
using System.Collections.Generic;
using DateTimeCompiler.Lexer.Tokens;

namespace DateTimeCompiler.Core.Expressions
{
    public class BinaryExpression: Expression
    {
        private readonly Dictionary<(Type, Type), Type> _typeRules;
        public Expression LeftExpression { get; }
        public Expression RightExpression { get; }

        public BinaryExpression(Token token, Expression leftExpression, Expression rightExpression) 
            : base(token, null)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
            _typeRules = new Dictionary<(Type, Type), Type>
            {
                {(Type.Date, Type.Year), Type.Date},
                {(Type.Date, Type.Month), Type.Date},
                {(Type.Date, Type.Day), Type.Date},
                {(Type.TimeStamp, Type.TimeStamp), Type.TimeStamp},
                {(Type.TimeStamp, Type.Hour), Type.TimeStamp},
                {(Type.TimeStamp, Type.Minute), Type.TimeStamp},
                {(Type.TimeStamp, Type.Second), Type.TimeStamp},
            };
        }

        public override dynamic Evaluate()
        {
            if (Token.TokenType== TokenType.Plus)
            {
                if (RightExpression.GetExpressionType()==Type.Hour)
                {
                    var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
                    var rightExpression = RightExpression.Evaluate();
                    return leftExpressionDate.AddHours(rightExpression);
                }
                return LeftExpression.Evaluate() + RightExpression.Evaluate();
            }
            else if(Token.TokenType == TokenType.Substract)
            {
                return LeftExpression.Evaluate() - RightExpression.Evaluate();
            }
            throw new ApplicationException($"Cannot perform Binary operation on {LeftExpression.GetExpressionType()}, {RightExpression.GetExpressionType()}");
        }

        public override Type GetExpressionType()
        {
            if (_typeRules.TryGetValue((LeftExpression.GetExpressionType(), RightExpression.GetExpressionType()), out var resultType))
            {
                return resultType;
            }
            throw new ApplicationException($"Cannot perform Binary operation on {LeftExpression.GetExpressionType()}, {RightExpression.GetExpressionType()}");
        }
    }
}