namespace GrammGen.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class GenerateRule : Rule
    {
        private Rule rule;
        private string type;
        private Func<object, object> func;

        public GenerateRule(Rule rule, string type)
            : this(rule, type, null)
        {
        }

        public GenerateRule(Rule rule, string type, Func<object, object> func)
        {
            this.rule = rule;
            this.type = type;
            this.func = func;
        }

        public override string Type
        {
            get
            {
                return this.type;
            }
        }

        public override Element Process(IParser source)
        {
            var result = this.rule.Process(source);

            if (result == null)
                return null;

            if (func != null)
                return new Element(this.type, func(result.Value));

            return new Element(this.type, result.Value);
        }
    }
}
