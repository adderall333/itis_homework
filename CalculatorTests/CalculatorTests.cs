using NUnit.Framework;
using System;
using homework_1;

namespace CalculatorTests
{
    [TestFixture]

    class CalculatorTests
    {
        private static readonly Calculator Calculator = new Calculator();
        
        [TestCase(1, "+", 2, 3, TestName = "One plus two equals three")]
        [TestCase(2, "-", 1, 1, TestName = "Two minus one equals one")]
        [TestCase(2, "*", 3, 6, TestName = "Two multiply three equals six")]
        [TestCase(4, "/", 2, 2, TestName = "Four divided two equals two")]
        [TestCase(5, "/", 2, 2.5, TestName = "Five divided two equals two and half")]
        [TestCase(5, "/", 0, double.PositiveInfinity, TestName = "Five divided zero equals infinity")]
        public void CalculateTests(double val1, string operation, double val2, double result)
        {
            Assert.AreEqual(result, Calculator.Calculate(val1, operation, val2));
        }

        [Test]
        public void WrongInputTest()
        {
            Assert.Throws<NotSupportedException>(() => Calculator.Calculate(5, "#", 3));
        }
    }
}
