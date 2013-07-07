﻿namespace GrammGen.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
        public void ProcessTwoStrings()
        {
            var rule = Rule.Get("abc", "def");

            var result = rule.Process("abcdef");

            Assert.IsNotNull(result);
            Assert.AreEqual("abcdef", result.Value);
        }

        [TestMethod]
        public void GetWithInvalidArgument()
        {
            try
            {
                Rule.Get(123);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
                Assert.AreEqual("Invalid rule argument", ex.Message);
            }
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

        [TestMethod]
        public void ProcessOrCharacters()
        {
            var rule = Rule.Or('a', 'b');

            var result = rule.Process("ac");

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);

            result = rule.Process("bc");

            Assert.IsNotNull(result);
            Assert.AreEqual("b", result.Value);
        }

        [TestMethod]
        public void ProcessOrStrings()
        {
            var rule = Rule.Or("abc", "abd");

            var result = rule.Process("abc");

            Assert.IsNotNull(result);
            Assert.AreEqual("abc", result.Value);

            result = rule.Process("abd");

            Assert.IsNotNull(result);
            Assert.AreEqual("abd", result.Value);
        }

        [TestMethod]
        public void ProcessOrCharacterRanges()
        {
            var rule = Rule.Or("a-z", "A-Z", "0-9");

            var result = rule.Process("g");

            Assert.IsNotNull(result);
            Assert.AreEqual("g", result.Value);

            result = rule.Process("M");

            Assert.IsNotNull(result);
            Assert.AreEqual("M", result.Value);

            result = rule.Process("7");

            Assert.IsNotNull(result);
            Assert.AreEqual("7", result.Value);
        }

        [TestMethod]
        public void OrWithInvalidArgument()
        {
            try
            {
                Rule.Or("abcdef", 123);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentException));
                Assert.AreEqual("Invalid rule argument", ex.Message);
            }
        }

        [TestMethod]
        public void GenerateInteger()
        {
            Rule rule = Rule.Get("0-9").OneOrMore().Generate("Integer");

            var result = rule.Process("123");

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual("Integer", result.Type);
        }

        [TestMethod]
        public void CombineStrings()
        {
            Assert.AreEqual("12", Rule.Combine("1", "2"));
        }

        [TestMethod]
        public void CombineIntegers()
        {
            var result = Rule.Combine(123, 456);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<object>));

            var list = (IList<object>)result;

            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(123, list[0]);
            Assert.AreEqual(456, list[1]);
        }

        [TestMethod]
        public void CombineArrayAndInteger()
        {
            var result = Rule.Combine(new object[] { 1, 2 }, 3);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<object>));

            var list = (IList<object>)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void CombineIntegerAndArray()
        {
            var result = Rule.Combine(1, new object[] { 2, 3 });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<object>));

            var list = (IList<object>)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }

        [TestMethod]
        public void CombineTwoArrays()
        {
            var result = Rule.Combine(new object[] { 1 }, new object[] { 2, 3 });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<object>));

            var list = (IList<object>)result;

            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(1, list[0]);
            Assert.AreEqual(2, list[1]);
            Assert.AreEqual(3, list[2]);
        }
    }
}
