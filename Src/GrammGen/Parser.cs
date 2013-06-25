﻿namespace GrammGen
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
        private IList<Rule> rules;

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
                this.rules = new List<Rule>(rules);
        }

        public Element Parse(string type)
        {
            if (this.rules == null)
                return null;

            foreach (var rule in this.rules.Where(r => r.Type == type))
            {
                var result = rule.Process(this);

                if (result != null)
                    return result;
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
