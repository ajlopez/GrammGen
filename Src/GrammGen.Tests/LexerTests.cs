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
            lexer.Get('a').IsA("Alpha");

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
            lexer.Get('a').IsA("Alpha");
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
            lexer.Get('a').Get('b').Or().IsA("AlphaBeta");

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
        public void GetLetterAWithSpaces()
        {
            Lexer lexer = new Lexer("  a   ");
            lexer.Get('a').IsA("Alpha");

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
            lexer.Get('a').IsA("Alpha");

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
            Assert.AreEqual("Alpha", result.Type);

            Assert.IsNull(lexer.NextToken());
        }
    }
}
