namespace GrammGen.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UpToRule : Rule
    {
        private Rule left;
        private Rule right;
        private bool combine;

        public UpToRule(Rule left, Rule right)
            : this(left, right, false)
        {
        }

        public UpToRule(Rule left, Rule right, bool combine)
        {
            this.left = left;
            this.right = right;
            this.combine = combine;
        }

        public override Element Process(IParser source)
        {
            var rleft = this.left.Process(source);

            if (rleft == null)
                return null;

            var result = string.Empty;

            var rright = this.right.Process(source);

            while (rright == null)
            {
                int ich = source.NextChar();

                if (ich < 0)
                    throw new Exception("unclosed term");

                result += (char)ich;
                rright = this.right.Process(source);
            }

            if (this.combine)
                return new Element(null, Rule.Combine(Rule.Combine(rleft.Value, result), rright.Value));

            return new Element(null, result);
        }
    }
}
