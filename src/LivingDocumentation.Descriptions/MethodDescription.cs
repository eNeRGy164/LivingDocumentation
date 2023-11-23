namespace LivingDocumentation;

[DebuggerDisplay("Method {ReturnType,nq} {Name,nq}")]
public class MethodDescription(string? returnType, string name) : MemberDescription(name), IHaveAMethodBody
{
    [DefaultValue("void")]
    public string ReturnType { get; } = returnType ?? "void";

    [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
    [JsonConverter(typeof(ConcreteTypeConverter<List<ParameterDescription>>))]
    public List<IParameterDescription> Parameters { get; } = [];

    public List<Statement> Statements { get; } = [];

    public override MemberType MemberType => MemberType.Method;

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        foreach (var statement in this.Statements)
        {
            statement.Parent = this;
        }
    }
}
