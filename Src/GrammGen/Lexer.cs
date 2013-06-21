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

        public Lexer(string text)
            : this(new StringReader(text))
        {
        }

        public Lexer(TextReader reader)
        {
            this.reader = reader;
        }

        public interface ILexerProcessor
        {
            string Process(char ch, Lexer lexer);
        }

        public static ILexerProcessor Or(params char[] chars)
        {
            return new OrProcessor(chars);
        }

        public void Define(string name, char ch)
        {
            this.Define(name, new CharacterProcessor(ch));
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
                string value = this.processors[name].Process(ch, this);

                if (value != null)
                    return new Token(name, value);
            }

            return null;
        }

        private int NextCharSkippingSpaces()
        {
            int ich;

            for (ich = this.NextChar(); ich >= 0; ich = this.NextChar())
                if (!char.IsWhiteSpace((char)ich))
                    break;

            return ich;
        }

        private int NextChar()
        {
            return this.reader.Read();
        }

        private class CharacterProcessor : ILexerProcessor
        {
            private char character;

            public CharacterProcessor(char character)
            {
                this.character = character;
            }

            public string Process(char ch, Lexer lexer)
            {
                if (this.character == ch)
                    return ch.ToString();

                return null;
            }
        }

        private class OrProcessor : ILexerProcessor
        {
            private char[] chars;

            public OrProcessor(char[] chars)
            {
                this.chars = chars;
            }

            public string Process(char ch, Lexer lexer)
            {
                if (this.chars.Contains(ch))
                    return ch.ToString();

                return null;
            }
        }
    }
}
