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
        public void ProcessFirstRule()
        {
            CharacterRule rule1 = new CharacterRule('a');
            CharacterRule rule2 = new CharacterRule('b');

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("ab");

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
        }

        [TestMethod]
        public void ProcessSecondRule()
        {
            CharacterRule rule1 = new CharacterRule('a');
            CharacterRule rule2 = new CharacterRule('b');

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("ba");

            Assert.IsNotNull(result);
            Assert.AreEqual("b", result.Value);
        }

        [TestMethod]
        public void RejectRules()
        {
            CharacterRule rule1 = new CharacterRule('a');
            CharacterRule rule2 = new CharacterRule('b');

            OrRule rule = new OrRule(rule1, rule2);

            var result = rule.Process("c");

            Assert.IsNull(result);
        }
    }
}
