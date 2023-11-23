namespace LivingDocumentation;

[DebuggerDisplay("AttributeArgument {Name} {Type} {Value}")]
public class AttributeArgumentDescription(string? name, string? type, string? value) : IAttributeArgumentDescription
{
    public string Name { get; } = name ?? throw new ArgumentNullException("name");

    public string Type { get; } = type ?? throw new ArgumentNullException("type");

    public string Value { get; } = value ?? throw new ArgumentNullException("value");
}
