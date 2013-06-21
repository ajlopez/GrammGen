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
        private IList<TokenProcessor> processors = new List<TokenProcessor>();
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

        public void Define(string type, ILexerProcessor processor)
        {
            this.processors.Add(new TokenProcessor(type, processor));
        }

        public Token NextToken()
        {
            int ich = this.NextCharSkippingSpaces();

            if (ich < 0)
                return null;

            char ch = (char)ich;

            foreach (var processor in this.processors)
            {
                var token = processor.NextToken(ch);

                if (token != null)
                    return token;
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

        private class TokenProcessor
        {
            private string type;
            private ILexerProcessor processor;

            public TokenProcessor(string type, ILexerProcessor processor)
            {
                this.type = type;
                this.processor = processor;
            }

            public Token NextToken(char ch)
            {
                var value = this.processor.Process(ch);

                if (value == null)
                    return null;

                return new Token(this.type, value);
            }
        }
    }
}
