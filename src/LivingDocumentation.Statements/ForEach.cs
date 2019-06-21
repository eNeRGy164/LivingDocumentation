using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("ForEach")]
    public class ForEach : Statement
    {
        public string Expression { get; set; }
    }
}
