using System;
using static Calculator;

namespace Homework_1
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var a = GetNumber();
            var @operator = Console.ReadLine();
            var b = GetNumber();
            int result = Calculate(a, @operator, b);
            Console.WriteLine(result);
        }
    }
}
