using System;

namespace homework_1
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var a = Calculator.GetNumber(Console.ReadLine());
            var @operator = Console.ReadLine();
            var b = Calculator.GetNumber(Console.ReadLine());
            var result = Calculator.Calculate(a, @operator, b);
            Console.WriteLine(result);
        }
    }
}
