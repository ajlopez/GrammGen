namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IParserProcessor
    {
        string Type { get; }

        ParserElement Process();
    }
}
