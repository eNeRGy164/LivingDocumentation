namespace LivingDocumentation;

[DebuggerDisplay("If")]
public class If : Statement
{
    public List<IfElseSection> Sections { get; } = [];

    [JsonIgnore]
    public override List<Statement> Statements => this.Sections.SelectMany(s => s.Statements).ToList();

    [OnDeserialized]
    internal new void OnDeserializedMethod(StreamingContext context)
    {
        foreach (var section in this.Sections)
        {
            section.Parent ??= this;
        }
    }
}
