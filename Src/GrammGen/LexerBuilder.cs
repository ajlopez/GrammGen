namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class LexerBuilder
    {
        private Lexer lexer;
        private IList<ILexerProcessor> processors = new List<ILexerProcessor>();

        public LexerBuilder(Lexer lexer)
        {
            this.lexer = lexer;
        }

        public LexerBuilder Get(char ch)
        {
            this.processors.Add(new CharacterProcessor(ch));

            return this;
        }

        public LexerBuilder Or()
        {
            this.processors = new List<ILexerProcessor>() { new OrProcessor(this.processors) };

            return this;
        }

        public LexerBuilder OneOrMany()
        {
            var processor = this.processors[this.processors.Count - 1];

            this.processors.RemoveAt(this.processors.Count - 1);

            this.processors.Add(new OneOrManyProcessor(this.lexer, processor));

            return this;
        }

        public LexerBuilder IsAn(string name)
        {
            return this.IsA(name);
        }

        public LexerBuilder IsA(string name)
        {
            if (this.processors.Count == 1)
                this.lexer.Define(name, this.processors[0]);
            else
                this.lexer.Define(name, new SequenceProcessor(this.lexer, this.processors));

            this.processors = new List<ILexerProcessor>();

            return this;
        }

        private class SequenceProcessor : ILexerProcessor
        {
            private Lexer lexer;
            private IList<ILexerProcessor> processors;

            public SequenceProcessor(Lexer lexer, IList<ILexerProcessor> processors)
            {
                this.lexer = lexer;
                this.processors = new List<ILexerProcessor>(processors);
            }

            public string Process(char ch)
            {
                string result = this.processors[0].Process(ch);

                if (result == null)
                    return null;

                Stack<int> consumed = new Stack<int>();

                foreach (var processor in this.processors.Skip(1))
                {
                    int ich = this.lexer.NextChar();

                    if (ich < 0)
                    {
                        this.lexer.PushChars(consumed);
                        return null;
                    }

                    consumed.Push(ich);

                    var result2 = processor.Process((char)ich);

                    if (result2 == null)
                    {
                        this.lexer.PushChars(consumed);
                        return null;
                    }

                    result += result2;
                }

                return result;
            }
        }

        private class CharacterProcessor : ILexerProcessor
        {
            private char character;

            public CharacterProcessor(char character)
            {
                this.character = character;
            }

            public string Process(char ch)
            {
                if (this.character == ch)
                    return ch.ToString();

                return null;
            }
        }

        private class OrProcessor : ILexerProcessor
        {
            private IList<ILexerProcessor> processors;

            public OrProcessor(IList<ILexerProcessor> processors)
            {
                this.processors = new List<ILexerProcessor>(processors);
            }

            public string Process(char ch)
            {
                foreach (var processor in this.processors)
                {
                    var result = processor.Process(ch);

                    if (result != null)
                        return result;
                }

                return null;
            }
        }

        private class OneOrManyProcessor : ILexerProcessor
        {
            private ILexerProcessor processor;
            private Lexer lexer;

            public OneOrManyProcessor(Lexer lexer, ILexerProcessor processor)
            {
                this.lexer = lexer;
                this.processor = processor;
            }

            public string Process(char ch)
            {
                var result = this.processor.Process(ch);

                if (result == null)
                    return null;

                for (int ich = this.lexer.NextChar(); ich >= 0; ich = this.lexer.NextChar())
                {
                    var newresult = this.processor.Process((char)ich);

                    if (newresult == null)
                    {
                        this.lexer.PushChar(ich);
                        break;
                    }

                    result += newresult;
                }

                return result;
            }
        }
    }
}
