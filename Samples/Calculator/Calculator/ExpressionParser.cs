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
            Rule.Or(' ', '\t', '\r', '\n').Skip(),
            Rule.Or('+', '-', '*', '/').Generate("Oper0"),
            Rule.Get("Expression", "Oper0", "Expression").Generate("Expression", MakeBinaryExpression),
            Rule.Get("Integer").Generate("Expression"),
            Rule.Get("0-9").OneOrMore().Generate("Integer", MakeIntegerConstantExpression),
            Rule.Get(Rule.Get('-'), Rule.Get("0-9").OneOrMore()).Generate("Integer", MakeIntegerConstantExpression)
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

        private static object MakeIntegerConstantExpression(object obj)
        {
            int value = int.Parse((string)obj);

            return new ConstantExpression(value);
        }

        private static object MakeBinaryExpression(object obj)
        {
            IList<object> values = (IList<object>)obj;

            return new BinaryExpression((IExpression)values[0], (string)values[1], (IExpression)values[2]);
        }
    }
}
