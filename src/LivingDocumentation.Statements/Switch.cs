namespace LivingDocumentation;

[DebuggerDisplay("Switch {Expression}")]
public class Switch : Statement
{
    public List<SwitchSection> Sections { get; } = new();

    public string? Expression { get; set; }

    [JsonIgnore]
    public override List<Statement> Statements => this.Sections.SelectMany(s => s.Statements).ToList();
}
