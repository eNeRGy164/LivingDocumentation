using System.Collections.Generic;
using System.Diagnostics;

namespace roslyn_uml
{
    [DebuggerDisplay("Method {ReturnType} {Name}")]
    public class MethodDescription : MemberDescription, IHaveAMethodBody
    {
        public string ReturnType { get; }
        public List<ParameterDescription> Parameters { get; } = new List<ParameterDescription>();
        public List<InvocationDescription> InvokedMethods { get; } = new List<InvocationDescription>();

        public MethodDescription(string returnType, string name)
                : base(MemberType.Method, name)
        {
            this.ReturnType = returnType;
        }
    }
}
