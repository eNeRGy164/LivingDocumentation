namespace LivingDocumentation;

[DebuggerDisplay("Attribute {Type} {Name}")]
public class AttributeDescription : IAttributeDescription
{
    public string Type { get; }

    public string Name { get; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    [JsonConverter(typeof(ConcreteTypeConverter<List<AttributeArgumentDescription>>))]
    public List<IAttributeArgumentDescription> Arguments { get; } = [];

    public AttributeDescription(string? type, string? name)
    {
        this.Type = type ?? throw new ArgumentNullException(nameof(type));
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
