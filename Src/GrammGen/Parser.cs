namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Parser : IParser
    {
        private TextReader reader;
        private Stack<Element> elements = new Stack<Element>();

        public Parser(string text)
            : this(new StringReader(text == null ? string.Empty : text))
        {
        }

        public Parser(TextReader reader)
        {
            this.reader = reader;
        }

        public int NextChar()
        {
            while (this.elements.Count > 0)
            {
                var element = this.elements.Peek();

                if (element.Value is int)
                {
                    this.elements.Pop();
                    return (int)element.Value;
                }

                if (element.Value is string)
                {
                    this.elements.Pop();

                    string value = (string)element.Value;

                    if (value == string.Empty)
                        continue;

                    char ch = value[0];
                    this.elements.Push(new Element(null, value.Substring(1)));

                    return ch;
                }

                return -2;
            }

            return this.reader.Read();
        }

        public void Push(int ich)
        {
            this.elements.Push(new Element(null, ich));
        }

        public void Push(string text)
        {
            this.elements.Push(new Element(null, text));
        }
    }
}
