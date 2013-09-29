namespace Calculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ConstantExpression : IExpression
    {
        private int value;

        public ConstantExpression(int value)
        {
            this.value = value;
        }

        public int Value { get { return this.value; } }

        public int Evaluate()
        {
            return this.value;
        }
    }
}
