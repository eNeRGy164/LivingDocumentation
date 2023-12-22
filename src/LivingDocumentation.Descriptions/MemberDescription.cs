namespace LivingDocumentation;

public abstract class MemberDescription(string name) : IMemberable
{
    public abstract MemberType MemberType { get; }

    public string Name { get; } = name ?? throw new ArgumentNullException("name");

    [DefaultValue(Modifier.Private)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
    public Modifier Modifiers { get; set; }

    [JsonIgnore]
    public bool IsInherited { get; internal set; } = false;

    [JsonConverter(typeof(ConcreteTypeConverter<DocumentationCommentsDescription>))]
    public IHaveDocumentationComments? DocumentationComments { get; set; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    [JsonConverter(typeof(ConcreteTypeConverter<List<AttributeDescription>>))]
    public List<IAttributeDescription> Attributes { get; } = [];

    public override bool Equals(object? obj)
    {
        if (obj is not MemberDescription other)
        {
            return false;
        }

        return Equals(this.MemberType, other.MemberType) && string.Equals(this.Name, other.Name);
    }

    public override int GetHashCode()
    {
        return (this.MemberType, this.Name).GetHashCode();
    }
}
