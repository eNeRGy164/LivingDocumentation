using System.Collections.Generic;
using System.Diagnostics;

namespace roslyn_uml
{
    [DebuggerDisplay("Invocation {Name} {ContainingType}")]
    public class InvocationDescription
    {
        public string ContainingType { get; }
        public string Name { get; }

        public InvocationDescription(string containingType, string name)
        {
            this.ContainingType = containingType;
            this.Name = name;
        }

        public List<ArgumentDescription> Arguments { get; } = new List<ArgumentDescription>();
    }
}