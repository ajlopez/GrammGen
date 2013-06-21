namespace GrammGen.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void GetLetterA()
        {
            Lexer lexer = new Lexer("a");
            lexer.Get('a').IsAn("Alpha");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
            Assert.AreEqual("Alpha", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetLetterAAndLetterB()
        {
            Lexer lexer = new Lexer("ab");
            lexer.Get('a').IsAn("Alpha");
            lexer.Get('b').IsA("Beta");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
            Assert.AreEqual("Alpha", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("b", result.Value);
            Assert.AreEqual("Beta", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetWordWithLettersAOrB()
        {
            Lexer lexer = new Lexer("ab");
            lexer.Get('a').Get('b').Or().IsAn("AlphaBeta");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
            Assert.AreEqual("AlphaBeta", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("b", result.Value);
            Assert.AreEqual("AlphaBeta", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetWordWithLettersAB()
        {
            Lexer lexer = new Lexer("ab");
            lexer.Get('a').Get('b').IsAn("AlphaBeta");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("ab", result.Value);
            Assert.AreEqual("AlphaBeta", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetWordWithLettersA()
        {
            Lexer lexer = new Lexer("aaaa");
            lexer.Get('a').OneOrMany().IsAn("Alphas");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("aaaa", result.Value);
            Assert.AreEqual("Alphas", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetWordWithLowerCaseLetters()
        {
            Lexer lexer = new Lexer("word");
            lexer.GetRange('a', 'z').OneOrMany().IsA("Word");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("word", result.Value);
            Assert.AreEqual("Word", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetInteger()
        {
            Lexer lexer = new Lexer("123");
            lexer.GetRange('0', '9').OneOrMany().IsAn("Integer");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual("Integer", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetReal()
        {
            Lexer lexer = new Lexer("123.456");
            lexer.GetRange('0', '9').OneOrMany().Get('.').GetRange('0', '9').OneOrMany().IsA("Real");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("123.456", result.Value);
            Assert.AreEqual("Real", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetIntegerAndReal()
        {
            Lexer lexer = new Lexer("123 123.456");
            lexer.GetRange('0', '9').OneOrMany().Get('.').GetRange('0', '9').OneOrMany().IsA("Real");
            lexer.GetRange('0', '9').OneOrMany().IsAn("Integer");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual("Integer", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("123.456", result.Value);
            Assert.AreEqual("Real", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetIntegerAndWord()
        {
            Lexer lexer = new Lexer("123 abc");
            lexer.GetRange('a', 'z').OneOrMany().IsA("Word");
            lexer.GetRange('0', '9').OneOrMany().IsAn("Integer");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual("Integer", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("abc", result.Value);
            Assert.AreEqual("Word", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetIntegerOperatorAndWord()
        {
            Lexer lexer = new Lexer("123+abc");
            lexer.GetRange('a', 'z').OneOrMany().IsA("Word");
            lexer.GetRange('0', '9').OneOrMany().IsAn("Integer");
            lexer.Get('+').IsAn("Operator");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual("Integer", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("+", result.Value);
            Assert.AreEqual("Operator", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("abc", result.Value);
            Assert.AreEqual("Word", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetWordWithLettersAOrLettersB()
        {
            Lexer lexer = new Lexer("aaaabb");
            lexer.Get('a').OneOrMany().IsAn("Alphas");
            lexer.Get('b').OneOrMany().IsAn("Betas");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("aaaa", result.Value);
            Assert.AreEqual("Alphas", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("bb", result.Value);
            Assert.AreEqual("Betas", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetLetterAWithSpaces()
        {
            Lexer lexer = new Lexer("  a   ");
            lexer.Get('a').IsAn("Alpha");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
            Assert.AreEqual("Alpha", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetLetterATabsAndSpaces()
        {
            Lexer lexer = new Lexer(" \ta\t  ");
            lexer.Get('a').IsAn("Alpha");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
            Assert.AreEqual("Alpha", result.Type);

            Assert.IsNull(lexer.NextToken());
        }

        [TestMethod]
        public void GetNames()
        {
            Lexer lexer = new Lexer("abc _123 _a1 abC1 A_name");
            lexer.Get('_').OrGetRange('0', '9').OrGetRange('a', 'z').OrGetRange('A', 'Z').OrGet('_').OneOrMany().IsA("Name");
            lexer.GetRange('a', 'z').OrGetRange('A', 'Z').GetRange('0', '9').OrGetRange('a', 'z').OrGetRange('A', 'Z').OrGet('_').ZeroOrMany().IsA("Name");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("abc", result.Value);
            Assert.AreEqual("Name", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("_123", result.Value);
            Assert.AreEqual("Name", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("_a1", result.Value);
            Assert.AreEqual("Name", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("abC1", result.Value);
            Assert.AreEqual("Name", result.Type);

            result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("A_name", result.Value);
            Assert.AreEqual("Name", result.Type);

            Assert.IsNull(lexer.NextToken());
        }
    }
}
