namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen.Rules;

    public abstract class Rule
    {
        public virtual string Type { get { return null; } }

        public static Rule Get(char ch)
        {
            return new CharacterRule(ch);
        }

        public static Rule Get(string text)
        {
            if (text.Length == 3 && text[1] == '-')
                return new CharacterRangeRule(text[0], text[2]);

            if (char.IsUpper(text[0]))
                return new ElementRule(text);

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

        public static Rule Or(params object[] arguments)
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
                    rule = new OrRule(rule, newrule);
            }

            return rule;
        }

        public static object Combine(object left, object right)
        {
            if (left is string && right is string)
                return (string)left + (string)right;

            return new List<object>() { left, right };
        }

        public Rule OneOrMore()
        {
            return new OneOrMoreRule(this);
        }

        public Rule ZeroOrMore()
        {
            return new ZeroOrMoreRule(this);
        }

        public Rule Generate(string type)
        {
            return new GenerateRule(this, type);
        }

        public Rule Generate(string type, Func<object, object> func)
        {
            return new GenerateRule(this, type, func);
        }

        public Element Process(string text)
        {
            return this.Process(new Parser(text));
        }

        public abstract Element Process(IParser source);
    }
}
