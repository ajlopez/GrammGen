namespace GrammGen.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void GetCharacters()
        {
            Parser source = new Parser("abc");

            Assert.AreEqual('a', source.NextChar());
            Assert.AreEqual('b', source.NextChar());
            Assert.AreEqual('c', source.NextChar());
            Assert.AreEqual(-1, source.NextChar());
        }

        [TestMethod]
        public void NextCharFromNull()
        {
            Parser source = new Parser((string)null);

            Assert.AreEqual(-1, source.NextChar());
        }
    }
}
