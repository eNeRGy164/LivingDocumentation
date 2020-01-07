using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Return {Expression}")]
    public class ReturnDescription : Statement
    {
        public string Expression
        {
            get;
        }

        public ReturnDescription(string expression)
        {
            this.Expression = expression;
        }
    }
}
