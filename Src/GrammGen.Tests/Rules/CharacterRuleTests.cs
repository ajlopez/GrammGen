namespace GrammGen.Tests.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen.Rules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CharacterRuleTests
    {
        [TestMethod]
        public void ProcessCharacter()
        {
            CharacterRule rule = new CharacterRule('a');

            var result = rule.Process("a");

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
        }

        [TestMethod]
        public void RejectCharacter()
        {
            CharacterRule rule = new CharacterRule('a');

            var result = rule.Process("b");

            Assert.IsNull(result);
        }

        [TestMethod]
        public void RejectEmptyString()
        {
            CharacterRule rule = new CharacterRule('a');

            var result = rule.Process(string.Empty);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void RejectNull()
        {
            CharacterRule rule = new CharacterRule('a');

            var result = rule.Process((string)null);

            Assert.IsNull(result);
        }
    }
}
