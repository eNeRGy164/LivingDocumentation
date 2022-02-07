namespace LivingDocumentation;

[DebuggerDisplay("Switch {Expression}")]
public class Switch : Statement
{
    public List<SwitchSection> Sections { get; } = new();

    public string? Expression { get; set; }

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
