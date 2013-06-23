namespace GrammGen.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CharacterRangeRule : Rule
    {
        private char from;
        private char to;

        public CharacterRangeRule(char from, char to)
        {
            this.from = from;
            this.to = to;
        }

        public override Element Process(IParser source)
        {
            int ich = source.NextChar();

            if (ich < 0 || (char)ich < this.from || (char)ich > this.to)
            {
                source.Push(ich);
                return null;
            }

            return new Element(null, ((char)ich).ToString());
        }
    }
}
