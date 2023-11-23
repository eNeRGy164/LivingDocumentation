namespace LivingDocumentation;

[DebuggerDisplay("Invocation \"{ContainingType,nq}.{Name,nq}\"")]
public class InvocationDescription(string containingType, string name) : Statement
{
    public string ContainingType { get; } = containingType ?? throw new ArgumentNullException(nameof(containingType));

    public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));

    public List<ArgumentDescription> Arguments { get; } = [];
}
