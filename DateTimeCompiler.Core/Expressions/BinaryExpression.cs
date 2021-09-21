using System;
using System.Collections.Generic;
using DateTimeCompiler.Lexer.Tokens;

namespace DateTimeCompiler.Core.Expressions
{
    public class BinaryExpression: Expression
    {
        private readonly Dictionary<(Type, Type), Type> _typeRules;
        private readonly Dictionary<Type, Func<DateTime, dynamic, dynamic>> _evaluationDictionary;
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
                //-----------------------------
                {(Type.TimeStamp, Type.TimeStamp), Type.TimeStamp},
                {(Type.TimeStamp, Type.Hour), Type.TimeStamp},
                {(Type.TimeStamp, Type.Minute), Type.TimeStamp},
                {(Type.TimeStamp, Type.Second), Type.TimeStamp},
            };
            _evaluationDictionary = new Dictionary<Type, Func<DateTime, dynamic, dynamic>>
            {
                [Type.Hour] = (leftExpression, rightExpression) => leftExpression.AddHours(rightExpression),
                [Type.Minute] = (leftExpression, rightExpression) => leftExpression.AddMinutes(rightExpression),
                [Type.Second] = (leftExpression, rightExpression) => leftExpression.AddSeconds(rightExpression),
                [Type.Year] = (leftExpression, rightExpression) => leftExpression.AddYears(rightExpression),
                [Type.Month] = (leftExpression, rightExpression) => leftExpression.AddMonths(rightExpression),
                [Type.Day] = (leftExpression, rightExpression) => leftExpression.AddDays(rightExpression),
            };
        }

        public override dynamic Evaluate()
        {
            var rightExpressionType = RightExpression.GetExpressionType();
            if (Token.TokenType== TokenType.Plus)
            {
                if (_evaluationDictionary.ContainsKey(rightExpressionType))
                {
                    return GetExpressionResult(1);
                }
                return LeftExpression.Evaluate() + RightExpression.Evaluate();
            }
            else if(Token.TokenType == TokenType.Substract)
            {
                if (_evaluationDictionary.ContainsKey(rightExpressionType))
                {
                    return GetExpressionResult(-1);
                }

                return LeftExpression.Evaluate() - RightExpression.Evaluate();
            }
            throw new ApplicationException($"Cannot perform Binary operation on {LeftExpression.GetExpressionType()}, {RightExpression.GetExpressionType()}");
        }


        private dynamic GetExpressionResult(int multiplier)
        {
            var rightExpressionType = RightExpression.GetExpressionType();
            var leftExpressionDate = LeftExpression.Evaluate() is DateTime ? (DateTime)LeftExpression.Evaluate() : default;
            var rightExpression = RightExpression.Evaluate();
            return _evaluationDictionary[rightExpressionType](leftExpressionDate, rightExpression * multiplier);
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