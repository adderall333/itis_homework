﻿using System;
using System.Linq;
using System.Net.Http;

namespace ClientExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            //var query = Console.ReadLine();
            Console.WriteLine(Calculator.CalculateAsync("(2+3)/12*7+8*9").Result);
        }
    }
}