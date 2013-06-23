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
        private Stack<int> chars = new Stack<int>();

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
            if (this.chars.Count > 0)
                return this.chars.Pop();

            if (this.position >= this.length)
                return -1;

            return this.text[this.position++];
        }

        public void Push(int ich)
        {
            this.chars.Push(ich);
        }
    }
}
