using System;
using System.Linq;
using System.Linq.Expressions;

namespace ClientExpressions
{
    public static class Calculator
    {
        private static char[] lessPriorityOperators = {'+', '-'};
        private static char[] morePriorityOperators = {'/', '*'};
        
        public static Result Calculate(string query)
        {
            var expressionTree = GetExpressionTree(query);
            return null;
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
    }
}