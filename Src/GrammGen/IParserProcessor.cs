namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IParserProcessor
    {
        string Name { get; set; }

        ParserElement Process();
    }
}
