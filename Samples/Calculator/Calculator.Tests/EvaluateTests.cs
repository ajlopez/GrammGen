namespace Calculator.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EvaluateTests
    {
        [TestMethod]
        public void EvaluateIntegers()
        {
            Assert.AreEqual(123, Evaluate("123"));
            Assert.AreEqual(-123, Evaluate("-123"));
        }

        [TestMethod]
        public void EvaluateAdd()
        {
            Assert.AreEqual(3, Evaluate("1+2"));
            Assert.AreEqual(7, Evaluate("3 + 4"));
        }

        [TestMethod]
        public void EvaluateSubtract()
        {
            Assert.AreEqual(1, Evaluate("2-1"));
            Assert.AreEqual(-1, Evaluate("3 - 4"));
        }

        [TestMethod]
        public void EvaluateMultiply()
        {
            Assert.AreEqual(6, Evaluate("2*3"));
            Assert.AreEqual(-12, Evaluate("-3 * 4"));
        }

        [TestMethod]
        public void EvaluateDivide()
        {
            Assert.AreEqual(0, Evaluate("2/3"));
            Assert.AreEqual(4, Evaluate("8 / 2"));
        }

        private static int Evaluate(string text)
        {
            ExpressionParser parser = new ExpressionParser(text);
            IExpression expression = parser.ParseExpression();

            int value = expression.Evaluate();

            Assert.IsNull(parser.ParseExpression());

            return value;
        }
    }
}
