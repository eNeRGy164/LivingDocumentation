namespace LivingDocumentation;

public interface IHaveAMethodBody : IHaveModifiers
{
    IHaveDocumentationComments? DocumentationComments { get; set; }

    string Name { get; }

    List<IParameterDescription> Parameters { get; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Objects)]
    List<Statement> Statements { get; }

    List<IAttributeDescription> Attributes { get; }
}
