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
        private IDictionary<string, ILexerProcessor> processors = new Dictionary<string, ILexerProcessor>();
        private Stack<int> chars = new Stack<int>();

        public Lexer(string text)
            : this(new StringReader(text))
        {
        }

        public Lexer(TextReader reader)
        {
            this.reader = reader;
        }

        public LexerBuilder Get(char ch)
        {
            return (new LexerBuilder(this)).Get(ch);
        }

        public LexerBuilder GetRange(char from, char to)
        {
            return (new LexerBuilder(this)).GetRange(from, to);
        }

        public void Define(string name, ILexerProcessor processor)
        {
            this.processors[name] = processor;
        }

        public Token NextToken()
        {
            int ich = this.NextCharSkippingSpaces();

            if (ich < 0)
                return null;

            char ch = (char)ich;

            foreach (var name in this.processors.Keys)
            {
                string value = this.processors[name].Process(ch);

                if (value != null)
                    return new Token(name, value);
            }

            return null;
        }

        internal int NextChar()
        {
            if (this.chars.Count > 0)
                return this.chars.Pop();

            return this.reader.Read();
        }

        internal void PushChar(int ich)
        {
            this.chars.Push(ich);
        }

        internal void PushChars(Stack<int> stack)
        {
            while (stack.Count > 0)
                this.chars.Push(stack.Pop());
        }

        private int NextCharSkippingSpaces()
        {
            int ich;

            for (ich = this.NextChar(); ich >= 0; ich = this.NextChar())
                if (!char.IsWhiteSpace((char)ich))
                    break;

            return ich;
        }
    }
}
