using System.Collections.Generic;
using System.Linq.Expressions;

namespace ClientExpressions
{
    public class ExpressionResult
    {
        private static Dictionary<Expression, ExpressionResult> Instances 
            = new Dictionary<Expression, ExpressionResult>();
        public Expression Expression { get; }
        public double Result;

        private ExpressionResult(Expression expression)
        {
            Expression = expression;
            if (expression is ConstantExpression constantExpression)
                Result = (int)constantExpression.Value;
        }

        public static ExpressionResult GetExpressionResult(Expression expression)
        {
            if (Instances.ContainsKey(expression))
                return Instances[expression];
            
            var newExpressionResult = new ExpressionResult(expression);
            Instances[expression] = newExpressionResult;
            return newExpressionResult;
        }
    }
}