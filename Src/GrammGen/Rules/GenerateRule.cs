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

        public GenerateRule(Rule rule, string type)
        {
            this.rule = rule;
            this.type = type;
        }

        public override Element Process(IParser source)
        {
            var result = this.rule.Process(source);

            if (result == null)
                return null;

            return new Element(this.type, result.Value);
        }
    }
}
