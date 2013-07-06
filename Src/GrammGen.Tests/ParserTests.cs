﻿namespace GrammGen.Tests
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
            Rule skip = Rule.Get(' ').Generate(Parser.Skip);
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
    }
}
