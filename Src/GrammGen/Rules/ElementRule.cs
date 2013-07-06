namespace GrammGen.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ElementRule : Rule
    {
        private string type;

        public ElementRule(string type)
        {
            this.type = type;
        }

        public override Element Process(IParser source)
        {
            return source.Parse(this.type);
        }
    }
}
