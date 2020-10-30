using System;
using System.IO;
using System.Linq.Expressions;
using homework_1;
using Microsoft.Extensions.DependencyInjection;

namespace ClientExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<ICalculator, RemoteCalculator>();
            services.AddSingleton<IExpressionMaker, ExpressionMaker>();
            services.AddSingleton<ILogger, ExpressionLogger>();
            services.AddSingleton<ExpressionVisitor, CalculatorExpressionVisitor>();
            services.AddSingleton(typeof(TextWriter), Console.Out);
            services.AddSingleton(typeof(TextReader), Console.In);
            (new Client(services)).Start();
        }
    }
}