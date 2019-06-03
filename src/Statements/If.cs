using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace roslyn_uml
{
    [DebuggerDisplay("If")]
    public class If : Statement
    {
        public List<IfElseSection> Sections { get; } = new List<IfElseSection>();

        [JsonIgnore]
        public override List<Statement> Statements => this.Sections.SelectMany(s => s.Statements).ToList();
    }
}
