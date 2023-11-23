namespace LivingDocumentation;

[DebuggerDisplay("Parameter {Type} {Name}")]
public class ParameterDescription(string type, string name) : IParameterDescription
{
    public string Type { get; } = type ?? throw new ArgumentNullException(nameof(type));

    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    public bool HasDefaultValue { get; set; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    [JsonConverter(typeof(ConcreteTypeConverter<List<AttributeDescription>>))]
    public List<IAttributeDescription> Attributes { get; } = [];
}
