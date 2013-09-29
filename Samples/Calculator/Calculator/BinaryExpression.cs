namespace Calculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BinaryExpression : IExpression
    {
        private IExpression left;
        private IExpression right;
        private string operation;

        public BinaryExpression(IExpression left, string operation, IExpression right)
        {
            this.left = left;
            this.right = right;
            this.operation = operation;
        }

        public IExpression Left { get { return this.left; } }

        public IExpression Right { get { return this.right; } }

        public string Operation { get { return this.operation; } }

        public int Evaluate()
        {
            int lvalue = this.left.Evaluate();
            int rvalue = this.right.Evaluate();

            if (this.operation == "+")
                return lvalue + rvalue;
            if (this.operation == "-")
                return lvalue - rvalue;
            if (this.operation == "*")
                return lvalue * rvalue;
            if (this.operation == "/")
                return lvalue / rvalue;

            throw new NotImplementedException();
        }
    }
}
