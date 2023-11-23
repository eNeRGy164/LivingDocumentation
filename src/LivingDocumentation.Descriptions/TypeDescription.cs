namespace LivingDocumentation;
 
[DebuggerDisplay("{Type} {Name,nq} ({Namespace,nq})")]
public class TypeDescription(TypeType type, string? fullName) : IHaveModifiers
{
    [JsonProperty(Order = 1, PropertyName = nameof(Fields))]
    private readonly List<FieldDescription> fields = [];

    [JsonProperty(Order = 2, PropertyName = nameof(Constructors))]
    private readonly List<ConstructorDescription> constructors = [];

    [JsonProperty(Order = 3, PropertyName = nameof(Properties))]
    private readonly List<PropertyDescription> properties = [];

    [JsonProperty(Order = 4, PropertyName = nameof(Methods))]
    private readonly List<MethodDescription> methods = [];

    [JsonProperty(Order = 5, PropertyName = nameof(EnumMembers))]
    private readonly List<EnumMemberDescription> enumMembers = [];

    [JsonProperty(Order = 6, PropertyName = nameof(Events))]
    private readonly List<EventDescription> events = [];

    public TypeType Type { get; } = type;

    public string FullName { get; } = fullName ?? string.Empty;

    public DocumentationCommentsDescription? DocumentationComments { get; set; }

    public List<string> BaseTypes { get; } = [];

    [DefaultValue(Modifier.Internal)]
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
    public Modifier Modifiers { get; set; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    [JsonConverter(typeof(ConcreteTypeConverter<List<AttributeDescription>>))]
    public List<IAttributeDescription> Attributes { get; } = [];

    [JsonIgnore]
    public string Name => this.FullName.ClassName();

    [JsonIgnore]
    public string Namespace => this.FullName.Namespace();

    [JsonIgnore]
    public IReadOnlyList<ConstructorDescription> Constructors => this.constructors;

    [JsonIgnore]
    public IReadOnlyList<PropertyDescription> Properties => this.properties;

    [JsonIgnore]
    public IReadOnlyList<MethodDescription> Methods => this.methods;

    [JsonIgnore]
    public IReadOnlyList<EventDescription> Events => this.events;

    [JsonIgnore]
    public IReadOnlyList<FieldDescription> Fields => this.fields;

    [JsonIgnore]
    public IReadOnlyList<EnumMemberDescription> EnumMembers => this.enumMembers;

    public void AddMember(MemberDescription member)
    {
        switch (member)
        {
            case ConstructorDescription c:
                this.constructors.Add(c);
                break;

            case FieldDescription f:
                this.fields.Add(f);
                break;

            case PropertyDescription p:
                this.properties.Add(p);
                break;

            case MethodDescription m:
                this.methods.Add(m);
                break;

            case EnumMemberDescription em:
                this.enumMembers.Add(em);
                break;

            case EventDescription e:
                this.events.Add(e);
                break;

            default:
                throw new NotSupportedException($"Unable to add {member.GetType()} as member");
        }
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TypeDescription other)
        {
            return false;
        }

        return string.Equals(this.FullName, other.FullName);
    }

    public override int GetHashCode() => this.FullName.GetHashCode();

    public IEnumerable<IHaveAMethodBody> MethodBodies() => this.Constructors.Cast<IHaveAMethodBody>().Concat(this.Methods);

    public bool ImplementsType(string fullName) => this.BaseTypes.Contains(fullName);

    public bool ImplementsTypeStartsWith(string partialName) => this.BaseTypes.Any(bt => bt.StartsWith(partialName, StringComparison.Ordinal));

    public bool IsClass() => this.Type == TypeType.Class;

    public bool IsEnum() => this.Type == TypeType.Enum;

    public bool IsInterface() => this.Type == TypeType.Interface;

    public bool IsStruct() => this.Type == TypeType.Struct;

    public bool HasProperty(string name) => this.properties.Any(m => string.Equals(m.Name, name, StringComparison.Ordinal));

    public bool HasMethod(string name) => this.methods.Any(m => string.Equals(m.Name, name, StringComparison.Ordinal));

    public bool HasEvent(string name) => this.events.Any(m => string.Equals(m.Name, name, StringComparison.Ordinal));

    public bool HasField(string name) => this.fields.Any(m => string.Equals(m.Name, name, StringComparison.Ordinal));

    public bool HasEnumMember(string name) => this.enumMembers.Any(m => string.Equals(m.Name, name, StringComparison.Ordinal));
}
