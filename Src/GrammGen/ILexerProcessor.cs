namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ILexerProcessor
    {
        string Process(char ch);
    }
}
