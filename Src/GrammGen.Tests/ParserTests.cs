namespace GrammGen.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParseIntegerAsExpression()
        {
            Lexer lexer = CreateLexer("123");
            Parser parser = new Parser(lexer);

            parser.Get("Integer").Then("ConstantExpression");
            parser.Get("ConstantExpression").Then("Expression");

            var result = parser.ParseElement("Expression");

            Assert.IsNotNull(result);
            Assert.AreEqual("Expression", result.Type);
            Assert.IsNotNull(result.Value);

            Assert.IsNull(parser.ParseElement("Expression"));
            Assert.IsNull(parser.NextElement());
        }

        [TestMethod]
        public void ParseOperator()
        {
            Lexer lexer = CreateLexer("+");
            Parser parser = new Parser(lexer);

            var result = parser.ParseElement("Operator");

            Assert.IsNotNull(result);
            Assert.AreEqual("Operator", result.Type);
            Assert.IsNotNull(result.Value);
            Assert.AreEqual("+", result.Value);

            Assert.IsNull(parser.ParseElement("Operator"));
            Assert.IsNull(parser.NextElement());
        }

        [TestMethod]
        public void ParseIntegerPlusNameAsExpression()
        {
            Lexer lexer = CreateLexer("123+foo");
            Parser parser = new Parser(lexer);

            parser.Get("Integer").Then("ConstantExpression");
            parser.Get("ConstantExpression").Then("TermExpression");
            parser.Get("Name").Then("TermExpression");
            parser.Get("TermExpression").Get("Operator", "+").Get("TermExpression").Then("Expression");

            var result = parser.ParseElement("Expression");

            Assert.IsNotNull(result);
            Assert.AreEqual("Expression", result.Type);
            Assert.IsNotNull(result.Value);

            Assert.IsNull(parser.ParseElement("Expression"));
            Assert.IsNull(parser.NextElement());
        }

        private static Lexer CreateLexer(string text)
        {
            Lexer lexer = new Lexer(text);

            lexer.GetRange('0', '9').OneOrMany().IsAn("Integer");
            lexer.GetRange('a', 'z').OrGetRange('A', 'Z')
                .GetRange('a', 'z').OrGetRange('A', 'Z').OrGetRange('0', '9').OrGet('_').IsA("Name");
            lexer.Get('_')
                .GetRange('a', 'z').OrGetRange('A', 'Z').OrGetRange('0', '9').OrGet('_').IsA("Name");
            lexer.Get('+').IsAn("Operator");
            lexer.Get('-').IsAn("Operator");
            lexer.Get('*').IsAn("Operator");
            lexer.Get('/').IsAn("Operator");
            lexer.Get('.').IsA("Separator");
            lexer.Get('[').IsA("Separator");
            lexer.Get(']').IsA("Separator");
            lexer.Get('(').IsA("Separator");
            lexer.Get(')').IsA("Separator");

            return lexer;
        }
    }
}
