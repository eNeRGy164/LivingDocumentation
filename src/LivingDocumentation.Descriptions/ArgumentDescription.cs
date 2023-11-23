namespace LivingDocumentation;

[DebuggerDisplay("Argument {Text} ({Type,nq})")]
public class ArgumentDescription(string type, string text)
{
    public string Type { get; } = type ?? throw new ArgumentNullException(nameof(type));

    public string Text { get; } = text ?? throw new ArgumentNullException(nameof(text));
}
