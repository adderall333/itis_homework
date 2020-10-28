using System;

namespace homework_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculator = new Calculator();
            
            var val1 = int.Parse(Console.ReadLine());
            var operation = Console.ReadLine();
            var val2 = int.Parse(Console.ReadLine());
            var result = calculator.Calculate(val1, operation, val2);
            Console.WriteLine(result);
        }
    }
}
