using System.Collections.Generic;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Method {ReturnType} {Name}")]
    public class MethodDescription : MemberDescription, IHaveAMethodBody
    {
        public string ReturnType { get; }

        public List<IParameterDescription> Parameters { get; } = new List<IParameterDescription>();

        public List<Statement> Statements { get; } = new List<Statement>();

        public MethodDescription(string returnType, string name)
                : base(MemberType.Method, name)
        {
            this.ReturnType = returnType;
        }
    }
}
