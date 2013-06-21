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
    }
}
