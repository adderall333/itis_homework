using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ClientExpressions
{
    public class ExpressionLogger : ILogger
    {
        private readonly List<string> logs;
        private readonly IExpressionMaker expressionMaker;
        
        private const string Space = "---";
        

        public ExpressionLogger(IExpressionMaker expressionMaker)
        {
            this.expressionMaker = expressionMaker;
            logs = new List<string>();
        }
        
        public void Log(object obj)
        {
            var expressionResult = (ExpressionResult) obj;
            
            if (expressionResult.Expression is ConstantExpression)
                logs.Add(expressionResult.Expression.ToString());
            
            if (expressionResult.Expression is BinaryExpression)
                logs.Add(string.Concat(Enumerable.Range(0, expressionResult.Ranking).Select(i => Space)) +
                         ">  " + expressionMaker.GetOperation(expressionResult.Expression));
        }

        public IEnumerable<string> GetLogs()
        {
            return logs;
        }

        public void Clear()
        {
            logs.Clear();
        }
    }
}