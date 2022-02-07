namespace LivingDocumentation;

public abstract class Statement
{
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Objects)]
    public virtual List<Statement> Statements { get; } = new();
}
