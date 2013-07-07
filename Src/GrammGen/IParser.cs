namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IParser
    {
        int NextChar();

        void Push(object obj);

        Element Parse(string type);
    }
}
