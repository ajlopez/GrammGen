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

        public Parser(Lexer lexer)
        {
            this.lexer = lexer;
        }

        public ParserBuilder Get(string name)
        {
            return (new ParserBuilder(this)).Get(name);
        }

        public ParserBuilder GetToken(string name)
        {
            return (new ParserBuilder(this)).GetToken(name);
        }

        public void Define(IParserProcessor processor)
        {
            this.processors.Add(processor);
        }

        public object Parse(string name)
        {
            foreach (var processor in this.processors.Where(p => p.Name == name))
            {
                var result = processor.Process();

                if (result != null)
                    return result.Value;
            }

            return null;
        }
    }
}
