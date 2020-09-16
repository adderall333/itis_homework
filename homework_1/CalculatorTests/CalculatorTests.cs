using NUnit.Framework;
using System;
using homework_1;

namespace CalculatorTests
{
    [TestFixture]

    class CalculatorTests
    {
        [TestCase(1, "+", 2, 3)]
        [TestCase(2, "-", 1, 1)]
        [TestCase(2, "*", 3, 6)]
        [TestCase(4, "/", 2, 2)]
        [TestCase(5, "/", 2, 2.5)]
        [TestCase(5, "/", 0, double.PositiveInfinity)]
        public void CalculateTests(double a, string @operator, double b, double result)
        {
            Assert.AreEqual(result, Calculator.Calculate(a, @operator, b));
        }

        [Test]
        public void WrongInputTest()
        {
            Assert.Throws<NotSupportedException>(() => Calculator.Calculate(5, "#", 3));
        }
    }
}
