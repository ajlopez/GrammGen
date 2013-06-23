namespace GrammGen.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OrRule : Rule
    {
        private Rule leftrule;
        private Rule rightrule;

        public OrRule(Rule leftrule, Rule rightrule)
        {
            this.leftrule = leftrule;
            this.rightrule = rightrule;
        }

        public override Element Process(IParser source)
        {
            var left = this.leftrule.Process(source);

            if (left != null)
                return left;

            var right = this.rightrule.Process(source);

            if (right != null)
                return right;

            return null;
        }
    }
}
