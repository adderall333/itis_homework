using System;

namespace homework_1
{
    public class Calculator : ICalculator
    {
        public double Calculate(double val1, string operation, double val2)
        {
            return operation switch
            {
                "+" => val1 + val2,
                "-" => val1 - val2,
                "*" => val1 * val2,
                "/" => val1 / val2,
                _ => throw new NotSupportedException()
            };
        }
    }
}
