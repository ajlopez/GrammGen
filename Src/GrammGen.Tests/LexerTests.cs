namespace GrammGen.Tests
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LexerTests
    {
        [TestMethod]
        public void GetLetterA()
        {
            Lexer lexer = new Lexer("a");
            lexer.Define("Alpha", 'a');

            var result = lexer.NextToken();

            Assert.IsNotNull(result);
            Assert.AreEqual("a", result.Value);
            Assert.AreEqual("Alpha", result.Type);

            Assert.IsNull(lexer.NextToken());
        }
    }
}
