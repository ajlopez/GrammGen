namespace Calculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using GrammGen;

    public class ExpressionParser
    {
        private static Rule[] rules = new Rule[] 
        {
            Rule.Get("0-9").OneOrMore().Generate("Expression", obj => new ConstantExpression(int.Parse((string)obj))),
            Rule.Get(Rule.Get('-'), Rule.Get("0-9").OneOrMore()).Generate("Expression", obj => new ConstantExpression(int.Parse((string)obj)))
        };

        private Parser parser;

        public ExpressionParser(string text)
        {
            this.parser = new Parser(text, rules);
        }

        public IExpression ParseExpression()
        {
            var element = this.parser.Parse("Expression");

            if (element == null)
                return null;

            return (IExpression)element.Value;
        }
    }
}
