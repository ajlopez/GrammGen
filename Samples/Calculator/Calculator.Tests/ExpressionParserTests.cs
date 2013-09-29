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

        [TestMethod]
        public void ParseIntegerWithSpaces()
        {
            ExpressionParser parser = new ExpressionParser("   123  ");

            IExpression expression = parser.ParseExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            ConstantExpression cexpression = (ConstantExpression)expression;

            Assert.AreEqual(123, cexpression.Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseIntegerWithOtherCharacters()
        {
            ExpressionParser parser = new ExpressionParser("   \t123\r\n  ");

            IExpression expression = parser.ParseExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            ConstantExpression cexpression = (ConstantExpression)expression;

            Assert.AreEqual(123, cexpression.Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseNegative()
        {
            ExpressionParser parser = new ExpressionParser("-123");

            IExpression expression = parser.ParseExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(ConstantExpression));

            ConstantExpression cexpression = (ConstantExpression)expression;

            Assert.AreEqual(-123, cexpression.Value);

            Assert.IsNull(parser.ParseExpression());
        }

        [TestMethod]
        public void ParseAdd()
        {
            ExpressionParser parser = new ExpressionParser("1+2");

            IExpression expression = parser.ParseExpression();

            Assert.IsNotNull(expression);
            Assert.IsInstanceOfType(expression, typeof(BinaryExpression));

            BinaryExpression bexpression = (BinaryExpression)expression;

            Assert.AreEqual("+", bexpression.Operation);
            Assert.IsNotNull(bexpression.Left);
            Assert.IsInstanceOfType(bexpression.Left, typeof(ConstantExpression));
            Assert.AreEqual(1, ((ConstantExpression)bexpression.Left).Value);
            Assert.IsNotNull(bexpression.Right);
            Assert.IsInstanceOfType(bexpression.Right, typeof(ConstantExpression));
            Assert.AreEqual(2, ((ConstantExpression)bexpression.Right).Value);

            Assert.IsNull(parser.ParseExpression());
        }
    }
}
