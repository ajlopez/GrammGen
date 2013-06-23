namespace GrammGen.Tests.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen.Rules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AndRuleTests
    {
        [TestMethod]
        public void ProcessTwoCharacterRules()
        {
            CharacterRule rule1 = new CharacterRule('a');
            CharacterRule rule2 = new CharacterRule('b');

            AndRule rule = new AndRule(rule1, rule2);

            var result = rule.Process("ab");

            Assert.IsNotNull(result);
            Assert.AreEqual("ab", result.Value);
        }
    }
}
