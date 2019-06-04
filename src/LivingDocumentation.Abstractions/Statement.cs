using System.Collections.Generic;

namespace LivingDocumentation
{
    public abstract class Statement
    {
        public virtual List<Statement> Statements { get; } = new List<Statement>();
    }
}
