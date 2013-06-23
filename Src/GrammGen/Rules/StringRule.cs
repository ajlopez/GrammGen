namespace GrammGen.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StringRule : Rule
    {
        private string text;

        public StringRule(string text)
        {
            this.text = text;
        }

        public override Element Process(ISource source)
        {
            int ich = source.NextChar();
            int k = 0;

            while (ich >= 0 && this.text[k] == ich)
            {
                k++;

                if (k >= this.text.Length)
                    break;

                ich = source.NextChar();
            }

            if (k >= this.text.Length)
                return new Element(null, this.text);

            source.Push(ich);

            if (k > 0)
                source.Push(this.text.Substring(0, k));

            return null;
        }
    }
}
