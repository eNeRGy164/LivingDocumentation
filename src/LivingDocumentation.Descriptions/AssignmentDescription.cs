namespace LivingDocumentation;

[DebuggerDisplay("Assignment \"{Left,nq} {Operator,nq} {Right,nq}\"")]
public class AssignmentDescription : Statement
{
    public string Left { get; }

    public string Operator { get; }

    public string Right { get; }

    public AssignmentDescription(string left, string @operator, string right)
    {
        this.Left = left;
        this.Operator = @operator;
        this.Right = right;
    }
}
