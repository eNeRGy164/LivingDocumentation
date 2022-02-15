namespace LivingDocumentation;

[DebuggerDisplay("Property {Type,nq} {Name,nq}")]
public class PropertyDescription : MemberDescription
{
    public string Type { get; }

    public string? Initializer { get; set; }
        
    [JsonIgnore]
    public bool HasInitializer => !string.IsNullOrWhiteSpace(this.Initializer);

    public override MemberType MemberType => MemberType.Property;

    public PropertyDescription(string type, string name)
        : base(name)
    {
        this.Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}
