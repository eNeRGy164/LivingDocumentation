namespace LivingDocumentation;

public abstract class Statement
{
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Objects)]
    public virtual List<Statement> Statements { get; } = new();

    [JsonIgnore]
    public object? Parent
    {
        get; internal set;
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        foreach (var statement in this.Statements)
        {
            statement.Parent ??= this;
        }
    }
}
