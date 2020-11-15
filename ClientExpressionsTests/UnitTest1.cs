using System;
using System.IO;
using System.Linq.Expressions;
using Xunit;
using homework_1;
using ClientExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace ClientExpressionsTests
{
    public class UnitTest1
    {
        public static homework_1.Calculator SimpleCalculator = new homework_1.Calculator();

        [Theory]
        [InlineData("(2+3)/12*7+8*9")]
        [InlineData("1,2+2,3")]
        [InlineData("(((((2+3)+3)+3)+3)+3)")]
        [InlineData("1/8+2/16+3/5")]
        [InlineData("(1000-1000)*5+8-3/2")]
        [InlineData("55+11*3/2*(15+8/3)")]
        [InlineData("27*5/2+(32-7*9)/((5+3)*8)")]
        [InlineData("(85-3*2/5)/22")]
        [InlineData("(8*19-32/4+66)")]
        [InlineData("(((((2+2)))))")]
        public void ExpressionsAreCompiledCorrectly(string query)
        {
            var services = new ServiceCollection();
            
            //here is dependency substitution
            services.AddSingleton<ICalculator, Calculator>();
            
            services.AddSingleton<IExpressionMaker, ExpressionMaker>();
            services.AddSingleton<ILogger, ExpressionLogger>();

            var compiledExpressionResult = Expression.Lambda<Func<double>>(
                new ExpressionMaker().GetExpressionTree(query)).Compile()();
            var actualResult = new Client(services).CalculateAsync(query).Result;
            
            Assert.Equal(compiledExpressionResult, actualResult);
        }
    }
}