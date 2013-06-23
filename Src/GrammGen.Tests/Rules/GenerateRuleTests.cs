namespace GrammGen.Tests.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen.Rules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GenerateRuleTests
    {
        [TestMethod]
        public void GetInteger()
        {
            GenerateRule rule = new GenerateRule(Rule.Get("0-9").OneOrMore(), "Integer");

            var result = rule.Process("123");

            Assert.IsNotNull(result);
            Assert.AreEqual("123", result.Value);
            Assert.AreEqual("Integer", result.Type);
        }

        [TestMethod]
        public void RejectLetter()
        {
            GenerateRule rule = new GenerateRule(Rule.Get("0-9").OneOrMore(), "Integer");

            var result = rule.Process("a");

            Assert.IsNull(result);
        }
    }
}
