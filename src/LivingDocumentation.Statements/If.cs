namespace LivingDocumentation;

[DebuggerDisplay("If")]
public class If : Statement
{
    public List<IfElseSection> Sections { get; } = new();

    [JsonIgnore]
    public override List<Statement> Statements => this.Sections.SelectMany(s => s.Statements).ToList();
}
