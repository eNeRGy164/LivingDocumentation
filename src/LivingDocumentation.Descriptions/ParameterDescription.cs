namespace LivingDocumentation;

[DebuggerDisplay("Parameter {Type} {Name}")]
public class ParameterDescription : IParameterDescription
{
    public string Type { get; }

    public string Name { get; }

    public bool HasDefaultValue { get; set; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    [JsonConverter(typeof(ConcreteTypeConverter<List<AttributeDescription>>))]
    public List<IAttributeDescription> Attributes { get; } = [];

    public ParameterDescription(string type, string name)
    {
        this.Type = type ?? throw new ArgumentNullException(nameof(type));
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
