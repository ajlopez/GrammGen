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

        public ParserBuilder Then(string type)
        {
            var newprocessor = new TransformerProcessor(this.processor, type);

            this.parser.Define(newprocessor);
            return this;
        }

        public ParserBuilder Get(string type)
        {
            this.processor = new ElementProcessor(this.parser, type);
            return this;
        }

        private class TransformerProcessor : IParserProcessor
        {
            private IParserProcessor processor;
            private string type;

            public TransformerProcessor(IParserProcessor processor, string type)
            {
                this.processor = processor;
                this.type = type;
            }

            public string Type { get { return this.type; } }

            public ParserElement Process()
            {
                var element = this.processor.Process();

                if (element == null)
                    return null;

                return new ParserElement(this.type, element.Value);
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

            public string Type { get; set; }

            public ParserElement Process()
            {
                var element = this.parser.NextElement();

                if (element != null)
                    if (element.Type == this.type)
                        return element;
                    else
                        this.parser.PushElement(element);

                element = this.parser.ParseElement(this.type);

                if (element == null)
                    return null;

                return new ParserElement(this.Type, element.Value);
            }
        }
    }
}
