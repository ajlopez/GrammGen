namespace GrammGen.Tests.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen.Rules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OrRuleTests
    {
        [TestMethod]
        public void ProcessFirstCharacterRule()
        {
            CharacterRule rule1 = new CharacterRule('a');
            CharacterRule rule2 = new CharacterRule('b');

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("ab");

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
        }

        [TestMethod]
        public void ProcessSecondCharacterRule()
        {
            CharacterRule rule1 = new CharacterRule('a');
            CharacterRule rule2 = new CharacterRule('b');

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("ba");

            Assert.IsNotNull(result);
            Assert.AreEqual("b", result.Value);
        }

        [TestMethod]
        public void RejectCharacterRules()
        {
            CharacterRule rule1 = new CharacterRule('a');
            CharacterRule rule2 = new CharacterRule('b');

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("c");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ProcessFirstCharacterRangeRule()
        {
            CharacterRangeRule rule1 = new CharacterRangeRule('a', 'z');
            CharacterRangeRule rule2 = new CharacterRangeRule('A', 'Z');

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("ab");

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
        }

        [TestMethod]
        public void ProcessSecondCharacterRangeRule()
        {
            CharacterRangeRule rule1 = new CharacterRangeRule('a', 'z');
            CharacterRangeRule rule2 = new CharacterRangeRule('A', 'Z');

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("BA");

            Assert.IsNotNull(result);
            Assert.AreEqual("B", result.Value);
        }

        [TestMethod]
        public void RejectCharacterRangeRules()
        {
            CharacterRangeRule rule1 = new CharacterRangeRule('a', 'z');
            CharacterRangeRule rule2 = new CharacterRangeRule('A', 'Z');

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("0");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ProcessFirstStringRule()
        {
            StringRule rule1 = new StringRule("abc");
            StringRule rule2 = new StringRule("abd");

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("abc");

            Assert.IsNotNull(result);
            Assert.AreEqual("abc", result.Value);
        }

        [TestMethod]
        public void ProcessSecondStringRule()
        {
            StringRule rule1 = new StringRule("abc");
            StringRule rule2 = new StringRule("abd");

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("abd");

            Assert.IsNotNull(result);
            Assert.AreEqual("abd", result.Value);
        }

        [TestMethod]
        public void RejectStringRules()
        {
            StringRule rule1 = new StringRule("a");
            StringRule rule2 = new StringRule("b");

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("c");

            Assert.IsNull(result);
        }
    }
}
