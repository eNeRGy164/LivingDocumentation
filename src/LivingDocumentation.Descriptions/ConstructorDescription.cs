namespace LivingDocumentation;

[DebuggerDisplay("Constructor {Name}")]
public class ConstructorDescription : MemberDescription, IHaveAMethodBody
{
    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    [JsonConverter(typeof(ConcreteTypeConverter<List<ParameterDescription>>))]
    public List<IParameterDescription> Parameters { get; } = new();

    public List<Statement> Statements { get; } = new();

    public override MemberType MemberType => MemberType.Constructor;

    public ConstructorDescription(string name)
        : base(name)
    {
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        foreach (var statement in this.Statements)
        {
            statement.Parent = this;
        }
    }
}
