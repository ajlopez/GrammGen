namespace GrammGen.Tests.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen.Rules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CharacterRangeRuleTests
    {
        [TestMethod]
        public void ProcessLowerCaseLetters()
        {
            CharacterRangeRule rule = new CharacterRangeRule('a', 'z');

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
    }
}
