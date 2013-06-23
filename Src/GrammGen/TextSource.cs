namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TextSource : ISource
    {
        private string text;
        private int position;
        private int length;
        private Stack<Element> elements = new Stack<Element>();

        public TextSource(string text)
        {
            this.text = text;
            this.position = 0;

            if (text == null)
                this.length = 0;
            else
                this.length = text.Length;
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

            if (this.position >= this.length)
                return -1;

            return this.text[this.position++];
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
