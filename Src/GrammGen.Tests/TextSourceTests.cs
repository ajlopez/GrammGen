namespace GrammGen.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TextSourceTests
    {
        [TestMethod]
        public void GetCharacters()
        {
            TextSource source = new TextSource("abc");

            Assert.AreEqual('a', source.NextChar());
            Assert.AreEqual('b', source.NextChar());
            Assert.AreEqual('c', source.NextChar());
            Assert.AreEqual(-1, source.NextChar());
        }

        [TestMethod]
        public void NextCharFromNull()
        {
            TextSource source = new TextSource(null);

            Assert.AreEqual(-1, source.NextChar());
        }
    }
}
