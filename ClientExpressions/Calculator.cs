using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ClientExpressions
{
    public static class Calculator
    {
        private static char[] lessPriorityOperators = {'+', '-'};
        private static char[] morePriorityOperators = {'/', '*'};
        
        public static async Task<double> CalculateAsync(string query)
        {
            var expressionTree = GetExpressionTree(query);
            var result = await ProcessInParallelAsync(expressionTree);
            return result.Result;
        }
        
        public static Expression GetExpressionTree(string str)
        {
            var i = 0;
            while (i < str.Length)
            {
                if (str[i] == '(')
                {
                    i = GoToNextScope(str, i);
                    if (i == -1)
                        return GetExpressionTree(str.Substring(1, str.Length - 2));
                }

                if (lessPriorityOperators.Contains(str[i]))
                {
                    var left = GetExpressionTree(str.Substring(0, i));
                    var right = GetExpressionTree(str.Substring(i + 1));
                    return GetExpression(left, str[i], right);
                }

                i++;
            }

            while (--i >= 0)
            {
                if (str[i] == ')')
                {
                    i = GoToPreviousScope(str, i);
                    if (i == -1)
                        return GetExpressionTree(str.Substring(1, str.Length - 2));
                }
                
                if (morePriorityOperators.Contains(str[i]))
                {
                    var left = GetExpressionTree(str.Substring(0, i));
                    var right = GetExpressionTree(str.Substring(i + 1));
                    return GetExpression(left, str[i], right);
                }
            }

            return Expression.Constant(int.Parse(str), typeof(int));
        }

        private static int GoToNextScope(string str, int i)
        {
            var openScopeIndex = i;
            var openCount = 1;
            var closeCount = 0;
            while (openCount > closeCount)
            {
                i++;
                openCount += str[i] == '(' ? 1 : 0;
                closeCount += str[i] == ')' ? 1 : 0;
            }

            if (i == str.Length - 1 && openScopeIndex == 0) 
                return -1;
            return i + 1;
        }

        private static int GoToPreviousScope(string str, int i)
        {
            var closeScopeIndex = i;
            var openCount = 0;
            var closeCount = 1;
            while (openCount < closeCount)
            {
                i--;
                openCount += str[i] == '(' ? 1 : 0;
                closeCount += str[i] == ')' ? 1 : 0;
            }

            if (i == 0 && closeScopeIndex == str.Length - 1) 
                return -1;
            return i - 1;
        }

        private static Expression GetExpression(Expression left, char operation, Expression right)
            => operation switch
            {
                '+' => Expression.Add(left, right),
                '-' => Expression.Subtract(left, right),
                '*' => Expression.Multiply(left, right),
                '/' => Expression.Divide(left, right),
                _ => throw new ArgumentException()
            };

        private static string GetOperation(Expression expression)
            => expression.NodeType switch
            {
                ExpressionType.Add => "+",
                ExpressionType.Subtract => "-",
                ExpressionType.Multiply => "*",
                ExpressionType.Divide => "/",
                _ => throw new ArgumentException()
            };

        private static async Task<ExpressionResult> ProcessInParallelAsync(Expression expression)
        {
            var space = "---";
            var visitor = new CalculatorExpressionVisitor();
            var lazy = new Dictionary<ExpressionResult, Lazy<Task>>();
            var executeBefore = visitor.GetExecuteBefore(expression);
            var res = executeBefore.Last().Key;
            foreach (var (exp, exps) in executeBefore)
            {
                lazy[exp] = new Lazy<Task>(async () =>
                {
                    if (exp.Expression is ConstantExpression)
                        Console.WriteLine(exp.Expression);
                    
                    if (exp.Expression is BinaryExpression)
                        Console.WriteLine(string.Concat(Enumerable.Range(0, exp.Ranking).Select(i => space)) + GetOperation(exp.Expression));
                    
                    await Task.WhenAll(exps.Select(e => lazy[e].Value));
                    await Task.Yield();
                    
                    if (exp.Expression is BinaryExpression)
                    {
                        var client = new HttpClient();
                        var response = await client.GetAsync(("http://localhost:5000/calculate?" +
                                                             $"val1={exps[0].Result}&" +
                                                             $"operation={GetOperation(exp.Expression)}&" +
                                                             $"val2={exps[1].Result}")
                                                             .Replace("+", "%2B"));

                        var result = response.Content.ReadAsStringAsync().Result;
                        var isDouble = Double.TryParse(result, out exp.Result);
                        
                        if (!isDouble)
                            throw new ArgumentException(result);
                    }
                });
            }

            await Task.WhenAll(lazy.Values.Select(l => l.Value));
            return res;
        }
    }
}