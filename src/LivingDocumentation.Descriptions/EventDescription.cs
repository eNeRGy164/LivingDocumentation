namespace LivingDocumentation;

[DebuggerDisplay("Event {Type,nq} {Name,nq}")]
public class EventDescription : MemberDescription
{
    public string Type { get; }

    public string? Initializer { get; set; }

    [JsonIgnore]
    public bool HasInitializer => this.Initializer is not null;

    public override MemberType MemberType => MemberType.Event;

    public EventDescription(string type, string name)
        : base(name)
    {
        this.Type = type;
    }
}
