namespace GrammGen.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RuleTests
    {
        [TestMethod]
        public void ProcessCharacter()
        {
            var rule = Rule.Get('a');

            var result = rule.Process("a");

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
        }

        [TestMethod]
        public void RejectCharacter()
        {
            var rule = Rule.Get('a');

            var result = rule.Process("b");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ProcessString()
        {
            var rule = Rule.Get("abc");

            var result = rule.Process("abc");

            Assert.IsNotNull(result);
            Assert.AreEqual("abc", result.Value);
        }

        [TestMethod]
        public void RejectString()
        {
            var rule = Rule.Get("abc");

            var result = rule.Process("def");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ProcessTwoCharacters()
        {
            var rule = Rule.Get('a', 'b');

            var result = rule.Process("ab");

            Assert.IsNotNull(result);
            Assert.AreEqual("ab", result.Value);
        }

        [TestMethod]
        public void ProcessLowerCaseLetters()
        {
            var rule = Rule.Get("a-z");

            var result = rule.Process("a");

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);

            for (char ch = 'b'; ch <= 'z'; ch++)
                Assert.IsNotNull(rule.Process(ch.ToString()));

            for (char ch = 'A'; ch <= 'Z'; ch++)
                Assert.IsNull(rule.Process(ch.ToString()));

            for (char ch = '0'; ch <= '9'; ch++)
                Assert.IsNull(rule.Process(ch.ToString()));
        }

        [TestMethod]
        public void ProcessIntegerUsingOneOrMore()
        {
            var rule = Rule.Get("0-9").OneOrMore();

            var result = rule.Process("123");

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
        }

        [TestMethod]
        public void ProcessIntegerUsingZeroOrMore()
        {
            var rule = Rule.Get("0-9").ZeroOrMore();

            var result = rule.Process("123");

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
        }

        [TestMethod]
        public void ProcessEmptyWordZeroOrMore()
        {
            var rule = Rule.Get("a-z").ZeroOrMore();

            var result = rule.Process("123");

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.Value);
        }
    }
}
