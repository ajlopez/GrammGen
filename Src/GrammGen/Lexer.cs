namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Lexer
    {
        private TextReader reader;
        private char ch;
        private string name;

        public Lexer(string text)
            : this(new StringReader(text))
        {
        }

        public Lexer(TextReader reader)
        {
            this.reader = reader;
        }

        public void Define(string name, char ch)
        {
            this.name = name;
            this.ch = ch;
        }

        public Token NextToken()
        {
            return null;
        }
    }
}
