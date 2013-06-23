﻿namespace GrammGen.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CharacterRule : Rule
    {
        private char character;

        public CharacterRule(char character)
        {
            this.character = character;
        }

        public override Element Process(ISource source)
        {
            int ich = source.NextChar();

            if (ich < 0 || (char)ich != this.character)
                return null;

            return new Element(null, ((char)ich).ToString());
        }
    }
}
