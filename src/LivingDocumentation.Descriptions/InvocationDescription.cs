namespace LivingDocumentation;

[DebuggerDisplay("Invocation \"{ContainingType,nq}.{Name,nq}\"")]
public class InvocationDescription : Statement
{
    public string ContainingType { get; }

    public string Name { get; }

    public List<ArgumentDescription> Arguments { get; } = new();

    public InvocationDescription(string containingType, string name)
    {
        this.ContainingType = containingType ?? throw new ArgumentNullException(nameof(containingType));
        this.Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
