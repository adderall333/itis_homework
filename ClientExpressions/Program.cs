using System;
using System.Linq;
using System.Net.Http;

namespace ClientExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            //var query = Console.ReadLine();
            Console.WriteLine(Calculator.CalculateAsync("(2+3*8)-5/3+34*9/2+(5-(4*7))/6").Result);
        }
    }
}