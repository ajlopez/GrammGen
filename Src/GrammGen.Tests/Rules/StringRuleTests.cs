namespace GrammGen.Tests.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen.Rules;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringRuleTests
    {
        [TestMethod]
        public void ProcessText()
        {
            StringRule rule = new StringRule("abc");

            var result = rule.Process("abc");

            Assert.IsNotNull(result);
            Assert.AreEqual("abc", result.Value);
        }

        [TestMethod]
        public void RejectText()
        {
            StringRule rule = new StringRule("abc");

            var result = rule.Process("ab");

            Assert.IsNull(result);
        }
    }
}
