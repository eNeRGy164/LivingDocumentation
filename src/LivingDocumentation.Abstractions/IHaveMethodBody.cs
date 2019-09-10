using System.Collections.Generic;

namespace LivingDocumentation
{
    public interface IHaveAMethodBody : IHaveModifiers
    {
        IHaveDocumentationComments DocumentationComments { get; set; }

        string Name { get; }

        List<IParameterDescription> Parameters { get; }

        List<Statement> Statements { get; }
    }
}
