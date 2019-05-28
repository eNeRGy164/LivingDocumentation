using System;
using System.Diagnostics;

namespace roslyn_uml.Uml
{
    [DebuggerDisplay("AltSection {GroupType} {Label}")]
    public class AltSection : InteractionFragment
    {
        public string GroupType { get; set; }
        public string Label { get; set; }
    }
}
