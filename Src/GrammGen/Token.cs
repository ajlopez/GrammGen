namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Token
    {
        private string type;
        private string value;

        public Token(string type, string value)
        {
            this.type = type;
            this.value = value;
        }

        public string Type { get { return this.type; } }

        public string Value { get { return this.value; } }
    }
}
