namespace LivingDocumentation;

[DebuggerDisplay("Method {ReturnType,nq} {Name,nq}")]
public class MethodDescription : MemberDescription, IHaveAMethodBody
{
    [DefaultValue("void")]
    public string ReturnType { get; }

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    [JsonConverter(typeof(ConcreteTypeConverter<List<ParameterDescription>>))]
    public List<IParameterDescription> Parameters { get; } = [];

    public List<Statement> Statements { get; } = [];

    public override MemberType MemberType => MemberType.Method;
        
    public MethodDescription(string? returnType, string name)
        : base(name)
    {
        this.ReturnType = returnType ?? "void";
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
