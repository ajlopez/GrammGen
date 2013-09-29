namespace Calculator.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Calculator");

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();

                line = line.Trim().ToLowerInvariant();

                if (line == "exit")
                    return;

                ExpressionParser parser = new ExpressionParser(line);

                for (IExpression expression = parser.ParseExpression(); expression != null; expression = parser.ParseExpression())
                    Console.WriteLine(expression.Evaluate());
            }
        }
    }
}
