using System;

namespace homework_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var val1 = int.Parse(Console.ReadLine());
            var operation = Console.ReadLine();
            var val2 = int.Parse(Console.ReadLine());
            var result = Calculator.Calculate(val1, operation, val2);
            Console.WriteLine(result);
        }
    }
}
