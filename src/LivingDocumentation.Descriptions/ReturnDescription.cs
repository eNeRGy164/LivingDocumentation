namespace LivingDocumentation;

[DebuggerDisplay("Return {Expression}")]
public class ReturnDescription(string expression) : Statement
{
    public string Expression { get; } = expression;
}
