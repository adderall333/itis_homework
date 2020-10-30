using System;
using Xunit;
using homework_1;
using ClientExpressions;

namespace ClientExpressionsTests
{
    public class UnitTest1
    {
        public static homework_1.Calculator SimpleCalculator = new homework_1.Calculator();

        [Theory]
        [InlineData("(2+3)/12*7+8*9")]
        public void ExpressionsAreCompiledCorrectly(string query)
        {
            //var 
        }
    }
}