namespace Calculator.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExpressionParserTests
    {
        [TestMethod]
        public void ParseInteger()
        {
            ExpressionParser parser = new ExpressionParser("123");

            IExpression expression = parser.ParseExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            ConstantExpression cexpression = (ConstantExpression)expression;

            Assert.AreEqual(123, cexpression.Value);

            Assert.IsNull(parser.ParseExpression());
        }
    }
}
