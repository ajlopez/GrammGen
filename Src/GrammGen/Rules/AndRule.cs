namespace GrammGen.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AndRule : Rule
    {
        private Rule leftrule;
        private Rule rightrule;

        public AndRule(Rule leftrule, Rule rightrule)
        {
            this.leftrule = leftrule;
            this.rightrule = rightrule;
        }

        public override Element Process(ISource source)
        {
            var left = this.leftrule.Process(source);

            if (left == null)
                return null;

            var right = this.rightrule.Process(source);

            if (right == null)
            {
                source.Push((string)left.Value);
                return null;
            }

            return new Element(null, (string)left.Value + (string)right.Value);
        }
    }
}
