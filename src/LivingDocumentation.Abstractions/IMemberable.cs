namespace LivingDocumentation;

public interface IMemberable : IHaveModifiers
{
    [JsonIgnore]
    MemberType MemberType { get; }

    [JsonProperty(Order = -3)]
    string Name { get; }

    IHaveDocumentationComments? DocumentationComments { get; internal set; }
}
