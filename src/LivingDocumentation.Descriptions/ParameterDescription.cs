namespace LivingDocumentation;

[DebuggerDisplay("Parameter {Type} {Name}")]
public class ParameterDescription : IParameterDescription
{
    public string Type { get; }

    public string Name { get; }

    public bool HasDefaultValue { get; set; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    [JsonConverter(typeof(ConcreteTypeConverter<List<AttributeDescription>>))]
    public List<IAttributeDescription> Attributes { get; } = new();

    public ParameterDescription(string type, string name)
    {
        this.Type = type;
        this.Name = name;
    }
}
