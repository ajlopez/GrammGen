namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ParserElement
    {
        private string type;
        private object value;

        public ParserElement(string type, object value)
        {
            this.type = type;
            this.value = value;
        }

        public string Type { get { return this.type; } }

        public object Value { get { return this.value; } }
    }
}
