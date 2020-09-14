using System;

namespace Homework_1
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var a = Calculator.GetNumber();
            var @operator = Console.ReadLine();
            var b = Calculator.GetNumber();
            var result = Calculator.Calculate(a, @operator, b);
            Console.WriteLine(result);
        }
    }
}
