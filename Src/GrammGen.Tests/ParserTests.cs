namespace GrammGen.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void GetCharacters()
        {
            Parser parser = new Parser("abc");

            Assert.AreEqual('a', parser.NextChar());
            Assert.AreEqual('b', parser.NextChar());
            Assert.AreEqual('c', parser.NextChar());
            Assert.AreEqual(-1, parser.NextChar());
        }

        [TestMethod]
        public void NextCharFromNull()
        {
            Parser parser = new Parser((string)null);

            Assert.AreEqual(-1, parser.NextChar());
        }

        [TestMethod]
        public void ParseWithoutRules()
        {
            Parser parser = new Parser("123");

            Assert.IsNull(parser.Parse("Integer"));
        }

        [TestMethod]
        public void ParseInteger()
        {
            Rule rule = Rule.Get("0-9").OneOrMore().Generate("Integer");
            Parser parser = new Parser("123", new Rule[] { rule });

            var result = parser.Parse("Integer");

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual("Integer", result.Type);
        }

        [TestMethod]
        public void ParseIntegerSkippingSpaces()
        {
            Rule rule = Rule.Get("0-9").OneOrMore().Generate("Integer");
            Rule skip = Rule.Get(' ').Skip();
            Parser parser = new Parser("  123  ", new Rule[] { rule, skip });

            var result = parser.Parse("Integer");

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual("Integer", result.Type);

            Assert.IsNull(parser.Parse("Integer"));
        }

        [TestMethod]
        public void ParseWordWithoutRule()
        {
            Rule rule = Rule.Get("0-9").OneOrMore().Generate("Integer");
            Parser parser = new Parser("123", new Rule[] { rule });

            Assert.IsNull(parser.Parse("Word"));
        }

        [TestMethod]
        public void ParseWord()
        {
            Rule rule = Rule.Get("0-9").OneOrMore().Generate("Integer");
            Rule rule2 = Rule.Get("Integer").Generate("Word");

            Parser parser = new Parser("123", new Rule[] { rule, rule2 });

            Assert.IsNotNull(parser.Parse("Word"));
        }

        [TestMethod]
        public void ParseAddIntegers()
        {
            Rule rule = Rule.Get("0-9").OneOrMore().Generate("Integer", x => int.Parse((string)x, System.Globalization.CultureInfo.InvariantCulture));
            Rule rule2 = Rule.Get("Integer", '+', "Integer").Generate("Add");

            Parser parser = new Parser("123+456", new Rule[] { rule, rule2 });

            var result = parser.Parse("Add");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(IList<object>));

            var list = (IList<object>)result.Value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(123, list[0]);
            Assert.AreEqual("+", list[1]);
            Assert.AreEqual(456, list[2]);

            Assert.IsNull(parser.Parse("Add"));
        }

        [TestMethod]
        public void ParseAddIntegersSkippingSpaces()
        {
            Rule skip = Rule.Get(' ').Skip();

            Rule rule = Rule.Get("0-9").OneOrMore().Generate("Integer", x => int.Parse((string)x, System.Globalization.CultureInfo.InvariantCulture));
            Rule rule2 = Rule.Get("Integer", '+', "Integer").Generate("Add");

            Parser parser = new Parser("  123  + 456  ", new Rule[] { skip, rule, rule2 });

            var result = parser.Parse("Add");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(IList<object>));

            var list = (IList<object>)result.Value;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(123, list[0]);
            Assert.AreEqual("+", list[1]);
            Assert.AreEqual(456, list[2]);

            Assert.IsNull(parser.Parse("Add"));
        }

        [TestMethod]
        public void ParseAddIntegersUsingLeftRecursion()
        {
            Rule ruleint = Rule.Get("0-9").OneOrMore().Generate("Integer", x => int.Parse((string)x, System.Globalization.CultureInfo.InvariantCulture));
            Rule ruleexpr = Rule.Get("Expression", '+', "Expression").Generate("Expression");
            Rule ruleintexpr = Rule.Get("Integer").Generate("Expression");

            Assert.AreEqual("Expression", ruleexpr.LeftType);

            Parser parser = new Parser("123+456+789", new Rule[] { ruleint, ruleexpr, ruleintexpr });

            var result = parser.Parse("Expression");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(IList<object>));

            var list = (IList<object>)result.Value;

            Assert.AreEqual(5, list.Count);
            Assert.AreEqual(123, list[0]);
            Assert.AreEqual("+", list[1]);
            Assert.AreEqual(456, list[2]);
            Assert.AreEqual("+", list[3]);
            Assert.AreEqual(789, list[4]);

            Assert.IsNull(parser.Parse("Expression"));
        }

        [TestMethod]
        public void ParseAddMultiplyIntegersUsingLeftRecursion()
        {
            Rule ruleint = Rule.Get("0-9").OneOrMore().Generate("Integer", x => int.Parse((string)x, System.Globalization.CultureInfo.InvariantCulture));
            Rule ruleexpr1 = Rule.Get("Expression", '+', "Term").Generate("Expression", MakeBinaryOperatorExpresion);
            Rule ruletermexpr1 = Rule.Get("Term").Generate("Expression");
            Rule ruletermexpr2 = Rule.Get("Term", '*', "Term").Generate("Term", MakeBinaryOperatorExpresion);
            Rule ruleintexpr = Rule.Get("Integer").Generate("Term");

            Assert.AreEqual("Expression", ruleexpr1.LeftType);
            Assert.AreEqual("Term", ruletermexpr2.LeftType);

            Parser parser = new Parser("123+456*789", new Rule[] { ruleint, ruleexpr1, ruletermexpr1, ruletermexpr2, ruleintexpr });

            var result = parser.Parse("Expression");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(BinaryOperatorExpression));

            var binop1 = (BinaryOperatorExpression)result.Value;

            Assert.AreEqual("+", binop1.Operator);
            Assert.AreEqual(123, binop1.Left);
            Assert.IsInstanceOfType(binop1.Right, typeof(BinaryOperatorExpression));

            var binop2 = (BinaryOperatorExpression)binop1.Right;

            Assert.AreEqual(456, binop2.Left);
            Assert.AreEqual("*", binop2.Operator);
            Assert.AreEqual(789, binop2.Right);

            Assert.IsNull(parser.Parse("Expression"));
        }

        [TestMethod]
        public void ParseMultiplyAddDivideIntegersUsingLeftRecursion()
        {
            Rule ruleint = Rule.Get("0-9").OneOrMore().Generate("Integer", x => int.Parse((string)x, System.Globalization.CultureInfo.InvariantCulture));
            Rule ruleleftexpr = Rule.Get("Expression", Rule.Or('+', '-'), "Term").Generate("Expression", MakeBinaryOperatorExpresion);
            Rule ruletermexpr = Rule.Get("Term").Generate("Expression"); 
            Rule rulelefterm = Rule.Get("Term", Rule.Or('*', '/'), "Factor").Generate("Term", MakeBinaryOperatorExpresion);
            Rule rulefactorterm = Rule.Get("Factor").Generate("Term");
            Rule ruleintfactor = Rule.Get("Integer").Generate("Factor");

            Parser parser = new Parser("123*456+789/10", new Rule[] { ruleint, ruleleftexpr, ruletermexpr, rulelefterm, rulefactorterm, ruleintfactor });

            var result = parser.Parse("Expression");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(BinaryOperatorExpression));

            var binop1 = (BinaryOperatorExpression)result.Value;

            Assert.AreEqual("+", binop1.Operator);
            Assert.IsInstanceOfType(binop1.Left, typeof(BinaryOperatorExpression));
            Assert.IsInstanceOfType(binop1.Right, typeof(BinaryOperatorExpression));

            var binop2 = (BinaryOperatorExpression)binop1.Left;

            Assert.AreEqual(123, binop2.Left);
            Assert.AreEqual("*", binop2.Operator);
            Assert.AreEqual(456, binop2.Right);

            var binop3 = (BinaryOperatorExpression)binop1.Right;

            Assert.AreEqual(789, binop3.Left);
            Assert.AreEqual("/", binop3.Operator);
            Assert.AreEqual(10, binop3.Right);

            Assert.IsNull(parser.Parse("Expression"));
        }

        [TestMethod]
        public void ParseExpressionWithParens()
        {
            Rule ruleint = Rule.Get("0-9").OneOrMore().Generate("Integer", x => int.Parse((string)x, System.Globalization.CultureInfo.InvariantCulture));
            Rule ruleleftexpr = Rule.Get("Expression", Rule.Or('+', '-'), "Term").Generate("Expression", MakeBinaryOperatorExpresion);
            Rule ruletermexpr = Rule.Get("Term").Generate("Expression");
            Rule rulelefterm = Rule.Get("Term", Rule.Or('*', '/'), "Factor").Generate("Term", MakeBinaryOperatorExpresion);
            Rule rulefactorterm = Rule.Get("Factor").Generate("Term");
            Rule ruleintfactor = Rule.Get("Integer").Generate("Factor");
            Rule ruleparens = Rule.Get('(', "Expression", ')').Generate("Factor", x => ((IList<object>)x)[1]);

            Parser parser = new Parser("123*(456+789)", new Rule[] { ruleint, ruleleftexpr, ruletermexpr, rulelefterm, rulefactorterm, ruleintfactor, ruleparens });

            var result = parser.Parse("Expression");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Value, typeof(BinaryOperatorExpression));

            var binop1 = (BinaryOperatorExpression)result.Value;

            Assert.AreEqual("*", binop1.Operator);
            Assert.AreEqual(123, binop1.Left);
            Assert.IsInstanceOfType(binop1.Right, typeof(BinaryOperatorExpression));

            var binop2 = (BinaryOperatorExpression)binop1.Right;

            Assert.AreEqual(456, binop2.Left);
            Assert.AreEqual("+", binop2.Operator);
            Assert.AreEqual(789, binop2.Right);

            Assert.IsNull(parser.Parse("Expression"));
        }

        private static object MakeBinaryOperatorExpresion(object obj)
        {
            var list = (IList<object>)obj;

            return new BinaryOperatorExpression()
            {
                Operator = (string)list[1],
                Left = list[0],
                Right = list[2]
            };
        }
    }
}
