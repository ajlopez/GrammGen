namespace GrammGen.Tests.Rules
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using GrammGen.Rules;

    [TestClass]
    public class UpToRuleTests
    {
        [TestMethod]
        public void ProcessQuotedString()
        {
            CharacterRule rule1 = new CharacterRule('"');
            UpToRule rule = new UpToRule(rule1, rule1);

            var result = rule.Process("\"foo\"");

            Assert.IsNotNull(result);
            Assert.AreEqual("foo", result.Value);
        }

        [TestMethod]
        public void ProcessQuotedStringIncludingQuotes()
        {
            CharacterRule rule1 = new CharacterRule('"');
            UpToRule rule = new UpToRule(rule1, rule1, true);

            var result = rule.Process("\"foo\"");

            Assert.IsNotNull(result);
            Assert.AreEqual("\"foo\"", result.Value);
        }

        [TestMethod]
        public void ProcessEmptyString()
        {
            CharacterRule rule1 = new CharacterRule('"');
            UpToRule rule = new UpToRule(rule1, rule1);

            var result = rule.Process("\"\"");

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.Value);
        }

        [TestMethod]
        public void ProcessLineComment()
        {
            StringRule rule1 = new StringRule("//");
            CharacterRule rule2 = new CharacterRule('\n');
            UpToRule rule = new UpToRule(rule1, rule2);

            var result = rule.Process("// a comment\n");

            Assert.IsNotNull(result);
            Assert.AreEqual(" a comment", result.Value);
        }

        [TestMethod]
        public void RejectInteger()
        {
            CharacterRule rule1 = new CharacterRule('"');
            UpToRule rule = new UpToRule(rule1, rule1);

            Assert.IsNull(rule.Process("123"));
        }

        [TestMethod]
        public void RaiseIfUnclosedString()
        {
            CharacterRule rule1 = new CharacterRule('"');
            UpToRule rule = new UpToRule(rule1, rule1);

            try
            {
                rule.Process("\"foo");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("unclosed term", ex.Message);
            }
        }
    }
}
