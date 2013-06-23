namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Element
    {
        private string type;
        private object value;

        public Element(string type, object value)
        {
            this.type = type;
            this.value = value;
        }

        public string Type { get { return this.type; } }

        public object Value { get { return this.value; } }
    }
}
