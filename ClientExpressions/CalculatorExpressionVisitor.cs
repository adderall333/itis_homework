using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientExpressions
{
    public class CalculatorExpressionVisitor : ExpressionVisitor
    {
        private readonly Dictionary<ExpressionResult, ExpressionResult[]> executeBefore =
            new Dictionary<ExpressionResult, ExpressionResult[]>();

        public Dictionary<ExpressionResult, ExpressionResult[]> GetExecuteBefore(Expression expression)
        {
            Visit(expression);
            return executeBefore;
        }
        
        protected override Expression VisitBinary(BinaryExpression binaryExpression)
        {
            Visit(binaryExpression.Left);
            Visit(binaryExpression.Right);

            executeBefore[ExpressionResult.GetExpressionResult(binaryExpression)] = new[]
            {
                ExpressionResult.GetExpressionResult(binaryExpression.Left), 
                ExpressionResult.GetExpressionResult(binaryExpression.Right)
            };

            return binaryExpression;
        }

        protected override Expression VisitConstant(ConstantExpression constantExpression)
        {
            executeBefore[ExpressionResult.GetExpressionResult(constantExpression)] = new ExpressionResult[0];
            return constantExpression;
        }
    }
}