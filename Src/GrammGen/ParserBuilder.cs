namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ParserBuilder
    {
        private Parser parser;

        public ParserBuilder(Parser parser)
        {
            this.parser = parser;
        }

        public ParserBuilder GetToken(string name)
        {
            return this;
        }

        public ParserBuilder IsA(string name)
        {
            return this;
        }

        public ParserBuilder Get(string name)
        {
            return this;
        }

        public ParserBuilder Then(string name)
        {
            return this;
        }
    }
}
