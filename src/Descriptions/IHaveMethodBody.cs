using System.Collections.Generic;

namespace roslyn_uml
{
    public interface IHaveAMethodBody : IHaveModifiers
    {
        string Name { get; }
        List<ParameterDescription> Parameters { get; }
        List<InvocationDescription> InvokedMethods { get; }
        List<Statement> Statements { get; }
    }
}
