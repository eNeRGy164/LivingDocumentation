using System.Collections.Generic;

namespace roslyn_uml
{
    public interface IHaveAMethodBody
    {
        string Name { get; }
        List<ParameterDescription> Parameters { get; }
        List<InvocationDescription> InvokedMethods { get; }
    }
}
