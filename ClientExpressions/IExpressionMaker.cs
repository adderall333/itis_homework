using System.Linq.Expressions;

namespace ClientExpressions
{
    public interface IExpressionMaker
    {
        public Expression GetExpressionTree(string str);
        public string GetOperation(Expression expression);
    }
}