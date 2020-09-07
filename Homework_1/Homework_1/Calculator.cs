using System;
using System.Linq;

namespace Homework_1
{
    partial class Program
    {
        class Calculator
        {
            static char[] operators = { '+', '-', '*', '/' };

            int result;

            void NextRequest(string request)
            {
                request = request.Replace(" ", string.Empty);
                var operatorIndex = 0;
                
                for (int i = 0; i < request.Length; i++)
                {
                    if (operators.Contains(request[i]))
                        operatorIndex = i;
                }

                var a = int.Parse(request.Substring(0, operatorIndex - 1));
                var b = int.Parse(request.Substring(operatorIndex + 1));
                result = MakeCalculation(a, b, request[operatorIndex]);
            }

            int MakeCalculation(int a, int b, char sign)
            {
                switch (sign)
                {
                    case '+':
                        return a + b;
                    case '-':
                        return a - b;
                    case '*':
                        return a * b;
                    case '/':
                        return a / b;
                    default:
                        throw new ArgumentException();
                }
            }
        }
    }
}
