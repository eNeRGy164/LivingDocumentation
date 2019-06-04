using System.Diagnostics;

namespace LivingDocumentation.Uml
{
    [DebuggerDisplay("AltSection {GroupType} {Label}")]
    public class AltSection : InteractionFragment
    {
        public string GroupType { get; set; }
        public string Label { get; set; }
    }
}
