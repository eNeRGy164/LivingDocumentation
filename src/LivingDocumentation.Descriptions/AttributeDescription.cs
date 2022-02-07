namespace LivingDocumentation;

[DebuggerDisplay("Attribute {Type} {Name}")]
public class AttributeDescription : IAttributeDescription
{
    public string Type { get; }

    public string Name { get; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    [JsonConverter(typeof(ConcreteTypeConverter<List<AttributeArgumentDescription>>))]
    public List<IAttributeArgumentDescription> Arguments { get; } = new();

    public AttributeDescription(string type, string name)
    {
        this.Type = type;
        this.Name = name;
    }
}
