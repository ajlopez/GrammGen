namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Parser
    {
        private Lexer lexer;
        private IList<IParserProcessor> processors = new List<IParserProcessor>();
        private Stack<ParserElement> elements = new Stack<ParserElement>();

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
        }

        public ParserBuilder Get(string name)
        {
            return (new ParserBuilder(this)).Get(name);
        }

        public void Define(IParserProcessor processor)
        {
            this.processors.Add(processor);
        }

        public ParserElement ParseElement(string type)
        {
            foreach (var processor in this.processors.Where(p => p.Type == type))
            {
                var result = processor.Process();

                if (result != null)
                    return result;
            }

            return null;
        }

        public ParserElement NextElement()
        {
            if (this.elements.Count > 0)
                return this.elements.Pop();

            var token = this.lexer.NextToken();

            if (token == null)
                return null;

            return new ParserElement(token.Type, token.Value);
        }

        public void PushElement(ParserElement element)
        {
            this.elements.Push(element);
        }
    }
}
