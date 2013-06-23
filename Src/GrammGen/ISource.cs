namespace GrammGen
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ISource
    {
        int NextChar();

        void Push(int ich);
    }
}
