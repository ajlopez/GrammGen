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
        private Stack<Token> tokens = new Stack<Token>();

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

        public ParserElement ParseElement(string type)
        {
            foreach (var processor in this.processors.Where(p => p.Name == type))
            {
                var result = processor.Process();

                if (result != null)
                    return result;
            }

            return null;
        }

        public Token NextToken()
        {
            if (this.tokens.Count > 0)
                return this.tokens.Pop();

            return this.lexer.NextToken();
        }

        public void PushToken(Token token)
        {
            this.tokens.Push(token);
        }
    }
}
