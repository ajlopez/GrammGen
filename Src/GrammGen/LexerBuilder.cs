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

                foreach (var processor in this.processors.Skip(1))
                {
                    int ch2 = this.lexer.NextChar();

                    if (ch2 < 0)
                        return null;

                    var result2 = processor.Process((char)ch2);

                    if (result2 == null)
                        return null;

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
    }
}
