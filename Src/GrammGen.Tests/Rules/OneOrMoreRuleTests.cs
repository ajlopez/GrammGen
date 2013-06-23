namespace GrammGen.Tests.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen.Rules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class OneOrMoreRuleTests
    {
        [TestMethod]
        public void GetLowerCaseLetters()
        {
            OneOrMoreRule rule = new OneOrMoreRule(new CharacterRangeRule('a', 'z'));

            var result = rule.Process("abcABC");

            Assert.IsNotNull(result);
            Assert.AreEqual("abc", result.Value);
        }

        [TestMethod]
        public void RejectEmptyString()
        {
            OneOrMoreRule rule = new OneOrMoreRule(new CharacterRangeRule('a', 'z'));

            var result = rule.Process(string.Empty);

            Assert.IsNull(result);
        }
    }
}
