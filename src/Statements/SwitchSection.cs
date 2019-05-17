using System.Collections.Generic;
using System.Diagnostics;

namespace roslyn_uml
{
    [DebuggerDisplay("Switch Section {Labels}")]
    public class SwitchSection : Statement
    {
        public List<string> Labels { get; } = new List<string>();
    }
}
