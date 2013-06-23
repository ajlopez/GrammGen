namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen.Rules;

    public abstract class Rule
    {
        public static Rule Get(char ch)
        {
            return new CharacterRule(ch);
        }

        public static Rule Get(string text)
        {
            return new StringRule(text);
        }

        public Element Process(string text)
        {
            return this.Process(new TextSource(text));
        }

        public abstract Element Process(ISource source);
    }
}
