namespace GrammGen.Tests.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen.Rules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ZeroOrMoreRuleTests
    {
        [TestMethod]
        public void GetLowerCaseLetters()
        {
            ZeroOrMoreRule rule = new ZeroOrMoreRule(new CharacterRangeRule('a', 'z'));

            var result = rule.Process("abcABC");

            Assert.IsNotNull(result);
            Assert.AreEqual("abc", result.Value);
        }

        [TestMethod]
        public void GetNoCharacter()
        {
            ZeroOrMoreRule rule = new ZeroOrMoreRule(new CharacterRangeRule('a', 'z'));

            var result = rule.Process("ABC");

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.Value);
        }
    }
}
