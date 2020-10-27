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

            var left = ExpressionResult.GetExpressionResult(binaryExpression.Left);
            var right = ExpressionResult.GetExpressionResult(binaryExpression.Right);
            
            executeBefore[ExpressionResult.GetExpressionResult(binaryExpression, 
                Math.Max(left.Ranking, right.Ranking))] = new[] {left, right};

            return binaryExpression;
        }

        protected override Expression VisitConstant(ConstantExpression constantExpression)
        {
            executeBefore[ExpressionResult.GetExpressionResult(constantExpression, -1)] = new ExpressionResult[0];
            return constantExpression;
        }
    }
}