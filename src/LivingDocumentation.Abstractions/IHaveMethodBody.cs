using System.Collections.Generic;

namespace LivingDocumentation
{
    public interface IHaveAMethodBody : IHaveModifiers
    {
        string Name { get; }

        List<IParameterDescription> Parameters { get; }

        List<Statement> Statements { get; }
    }
}
