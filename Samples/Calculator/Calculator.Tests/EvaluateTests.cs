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
