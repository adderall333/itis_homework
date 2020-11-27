using System;
using System.Linq.Expressions;

namespace MyExpressionVisitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var constant = Expression.Constant(5);
            var parameter = Expression.Parameter(typeof(int));
            var binary = Expression.MakeBinary(ExpressionType.Add, constant, parameter);
            var result = MyExpressionVisitor.Visit(binary);
        }
    }
}