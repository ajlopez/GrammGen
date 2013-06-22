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
            if (this.processor != null)
                this.processor = new AndProcessor(type, this.parser, this.processor, new ElementProcessor(this.parser, type));
            else
                this.processor = new ElementProcessor(this.parser, type);

            return this;
        }

        public ParserBuilder Get(string type, object value)
        {
            if (this.processor != null)
                this.processor = new AndProcessor(type, this.parser, this.processor, new ElementProcessor(this.parser, type, value));
            else
                this.processor = new ElementProcessor(this.parser, type, value);

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
            private object value;

            public ElementProcessor(Parser parser, string type)
                : this(parser, type, null)
            {
            }

            public ElementProcessor(Parser parser, string type, object value)
            {
                this.parser = parser;
                this.type = type;
                this.value = value;
            }

            public string Type { get { return this.type; } }

            public ParserElement Process()
            {
                var element = this.parser.NextElement();

                if (element != null)
                    if (element.Type == this.type && (this.value == null || this.value.Equals(element.Value)))
                        return element;
                    else
                        this.parser.PushElement(element);

                return this.parser.ParseElement(this.type);
            }
        }

        private class AndProcessor : IParserProcessor
        {
            private IParserProcessor lprocessor;
            private IParserProcessor rprocessor;
            private string type;
            private Parser parser;

            public AndProcessor(string type, Parser parser, IParserProcessor lprocessor, IParserProcessor rprocessor)
            {
                this.type = type;
                this.lprocessor = lprocessor;
                this.rprocessor = rprocessor;
                this.parser = parser;
            }

            public string Type { get; set; }

            public ParserElement Process()
            {
                var lelement = this.lprocessor.Process();

                if (lelement == null)
                    return null;

                var relement = this.rprocessor.Process();

                if (relement == null)
                {
                    this.parser.PushElement(lelement);
                    return null;
                }

                return new ParserElement(this.type, new List<ParserElement>() { lelement, relement });
            }
        }
    }
}
