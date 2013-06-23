namespace GrammGen.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ZeroOrMoreRule : Rule
    {
        private Rule rule;

        public ZeroOrMoreRule(Rule rule)
        {
            this.rule = rule;
        }

        public override Element Process(ISource source)
        {
            var result = new Element(null, string.Empty);

            for (var newresult = this.rule.Process(source); newresult != null; newresult = this.rule.Process(source))
                result = new Element(null, (string)result.Value + (string)newresult.Value);

            return result;
        }
    }
}
