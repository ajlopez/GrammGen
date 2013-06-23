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

        public static Rule Get(params object[] arguments)
        {
            Rule rule = null;

            foreach (var argument in arguments)
            {
                Rule newrule;

                if (argument is char)
                    newrule = Get((char)argument);
                else if (argument is string)
                    newrule = Get((string)argument);
                else
                    throw new ArgumentException("Invalid rule argument");

                if (rule == null)
                    rule = newrule;
                else
                    rule = new AndRule(rule, newrule);
            }

            return rule;
        }

        public Element Process(string text)
        {
            return this.Process(new TextSource(text));
        }

        public abstract Element Process(ISource source);
    }
}
