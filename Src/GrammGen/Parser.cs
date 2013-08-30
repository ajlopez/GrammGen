namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class Parser : IParser
    {
        public const string Skip = "_Skip";

        private TextReader reader;
        private Stack<Element> elements = new Stack<Element>();
        private IDictionary<string, IList<Rule>> rules = new Dictionary<string, IList<Rule>>();
        private IDictionary<string, IList<Rule>> leftrules = new Dictionary<string, IList<Rule>>();
        private IList<Rule> skips = new List<Rule>();

        public Parser(string text)
            : this(new StringReader(text == null ? string.Empty : text))
        {
        }

        public Parser(string text, IEnumerable<Rule> rules)
            : this(new StringReader(text == null ? string.Empty : text), rules)
        {
        }

        public Parser(TextReader reader)
            : this(reader, null)
        {
            this.reader = reader;
        }

        public Parser(TextReader reader, IEnumerable<Rule> rules)
        {
            this.reader = reader;

            if (rules != null)
                foreach (var rule in rules)
                    if (rule.Type == Skip)
                        this.skips.Add(rule);
                    else if (rule.LeftType == rule.Type)
                    {
                        if (!this.leftrules.ContainsKey(rule.Type))
                            this.leftrules[rule.Type] = new List<Rule>();

                        this.leftrules[rule.Type].Add(rule);
                    }
                    else
                    {
                        if (!this.rules.ContainsKey(rule.Type))
                            this.rules[rule.Type] = new List<Rule>();

                        this.rules[rule.Type].Add(rule);
                    }
        }

        public Element Parse(string type)
        {
            if (this.rules == null)
                return null;

            if (this.elements.Count > 0 && this.elements.Peek() is Element && ((Element)this.elements.Peek()).Type == type)
                return this.elements.Pop();

            while (this.ProcessSkipRules())
            {
            }

            if (this.rules.ContainsKey(type))
                foreach (var rule in this.rules[type])
                {
                    var result = rule.Process(this);

                    if (result != null)
                    {
                        while (this.ProcessSkipRules())
                        {
                        }

                        var newresult = this.ProcessLeftRecursionRules(type, result);

                        while (newresult != null)
                        {
                            result = newresult;
                            newresult = this.ProcessLeftRecursionRules(type, result);
                        }

                        return result;
                    }
                }

            return null;
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

        public void Push(object obj)
        {
            if (obj is Element)
                this.elements.Push((Element)obj);
            else
                this.elements.Push(new Element(null, obj));
        }

        private bool ProcessSkipRules()
        {
            foreach (var rule in this.skips)
            {
                var result = rule.Process(this);

                if (result != null)
                    return true;
            }

            return false;
        }

        private Element ProcessLeftRecursionRules(string type, Element result)
        {
            if (!this.leftrules.ContainsKey(type))
                return null;

            this.Push(result);

            foreach (var rule in this.leftrules[type])
            {
                var newresult = rule.Process(this);

                if (newresult != null)
                    return newresult;
            }

            this.elements.Pop();

            return null;
        }
    }
}
