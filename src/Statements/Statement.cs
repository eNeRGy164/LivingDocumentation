using System;
using System.Collections.Generic;

namespace roslyn_uml
{
    public abstract class Statement
    {
        public virtual List<Statement> Statements { get; } = new List<Statement>();
    }
}
