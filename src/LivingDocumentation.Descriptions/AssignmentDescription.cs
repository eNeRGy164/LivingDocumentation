namespace LivingDocumentation;

[DebuggerDisplay("Assignment \"{Left,nq} {Operator,nq} {Right,nq}\"")]
public class AssignmentDescription(string left, string @operator, string right) : Statement
{
    public string Left { get; } = left ?? throw new ArgumentNullException(nameof(left));

    public string Operator { get; } = @operator ?? throw new ArgumentNullException(nameof(@operator));

    public string Right { get; } = right ?? throw new ArgumentNullException(nameof(right));
}
