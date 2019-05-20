using System;
using System.Diagnostics;

namespace roslyn_uml
{
    [DebuggerDisplay("IfElse {Condition}")]
    public class IfElseSection : Statement
    {
        public string Condition { get; set; }
    }
}
