using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using homework_1;
using Microsoft.Extensions.DependencyInjection;

namespace ClientExpressions
{
    public class Client
    {
        private static ServiceProvider _serviceProvider;

        public Client(ServiceCollection services)
        {
            _serviceProvider = services.BuildServiceProvider();
        }

        public void Start()
        {
            var writer = _serviceProvider.GetService<TextWriter>();
            var reader = _serviceProvider.GetService<TextReader>();
            var logger = _serviceProvider.GetService<ILogger>();
            
            while (true)
            {
                var input = reader.ReadLine();
                if (input == "exit") break;
                writer.WriteLine($"answer: {CalculateAsync(input).Result}");
                writer.WriteLine("plan:");
                foreach (var log in logger.GetLogs())
                    writer.WriteLine(log);
                logger.Clear();
            }
        }
        
        public async Task<double> CalculateAsync(string query)
        {
            var expressionMaker = _serviceProvider.GetService<IExpressionMaker>();

            var expressionTree = expressionMaker.GetExpressionTree(query);
            var result = await ProcessInParallelAsync(expressionTree);
            return result.Result;
        }

        private static async Task<ExpressionResult> ProcessInParallelAsync(Expression expression)
        {
            var calculator = _serviceProvider.GetService<ICalculator>();
            var expressionMaker = _serviceProvider.GetService<IExpressionMaker>();
            var logger = _serviceProvider.GetService<ILogger>();
            
            var visitor = new CalculatorExpressionVisitor();
            var lazy = new Dictionary<ExpressionResult, Lazy<Task>>();
            var executeBefore = visitor.GetExecuteBefore(expression);
            var res = executeBefore.Last().Key;
            foreach (var (exp, exps) in executeBefore)
            {
                lazy[exp] = new Lazy<Task>(async () =>
                {
                    logger.Log(exp);
                    
                    await Task.WhenAll(exps.Select(e => lazy[e].Value));
                    await Task.Yield();
                    
                    if (exp.Expression is BinaryExpression)
                    {
                        exp.Result = calculator.Calculate(exps[0].Result, 
                            expressionMaker.GetOperation(exp.Expression), exps[1].Result);
                    }
                });
            }

            await Task.WhenAll(lazy.Values.Select(l => l.Value));
            return res;
        }
    }
}