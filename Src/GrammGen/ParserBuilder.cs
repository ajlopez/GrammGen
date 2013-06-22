namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ParserBuilder
    {
        private Parser parser;
        private IParserProcessor processor;

        public ParserBuilder(Parser parser)
        {
            this.parser = parser;
        }

        public ParserBuilder GetToken(string type)
        {
            this.processor = new TokenProcessor(this.parser, type);
            return this;
        }

        public ParserBuilder Then(string type)
        {
            this.processor.Name = type;

            this.parser.Define(this.processor);
            return this;
        }

        public ParserBuilder Get(string type)
        {
            this.processor = new ElementProcessor(this.parser, type);
            return this;
        }

        private class TokenProcessor : IParserProcessor
        {
            private Parser parser;
            private string type;

            public TokenProcessor(Parser parser, string type)
            {
                this.parser = parser;
                this.type = type;
            }

            public string Name { get; set; }

            public ParserElement Process()
            {
                Token token = this.parser.NextToken();

                if (token != null && token.Type == this.type)
                    return new ParserElement(this.type, token);

                this.parser.PushToken(token);

                return null;
            }
        }

        private class ElementProcessor : IParserProcessor
        {
            private Parser parser;
            private string type;

            public ElementProcessor(Parser parser, string type)
            {
                this.parser = parser;
                this.type = type;
            }

            public string Name { get; set; }

            public ParserElement Process()
            {
                var element = this.parser.ParseElement(this.type);

                if (element == null)
                    return null;

                return new ParserElement(this.Name, element.Value);
            }
        }
    }
}
