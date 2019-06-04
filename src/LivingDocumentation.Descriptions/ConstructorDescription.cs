using System.Collections.Generic;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Constructor {Name}")]
    public class ConstructorDescription : MemberDescription, IHaveAMethodBody
    {
        public List<IParameterDescription> Parameters { get; } = new List<IParameterDescription>();

        public List<Statement> Statements { get; } = new List<Statement>();

        public ConstructorDescription(string name)
            : base(MemberType.Constructor, name)
        {
        }
    }
}