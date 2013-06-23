namespace GrammGen.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OneOrMoreRule : Rule
    {
        private Rule rule;

        public OneOrMoreRule(Rule rule)
        {
            this.rule = rule;
        }

        public override Element Process(ISource source)
        {
            var result = this.rule.Process(source);

            if (result == null)
                return null;

            for (var newresult = this.rule.Process(source); newresult != null; newresult = this.rule.Process(source))
                result = new Element(null, (string)result.Value + (string)newresult.Value);

            return result;
        }
    }
}
