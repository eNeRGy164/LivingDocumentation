using System.Diagnostics;

namespace LivingDocumentation.Uml
{
    [DebuggerDisplay("{Source} -> {Target} : {Name}")]
    public class Arrow : InteractionFragment
    {
        public string Source { get; set; }
        public string Target { get; set; }
        public string Color { get; set; }
        public bool Dashed { get; set; }
        public string Name { get; set; }
    }
}
