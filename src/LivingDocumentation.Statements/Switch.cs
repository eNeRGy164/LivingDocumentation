using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LivingDocumentation
{
    [DebuggerDisplay("Switch {Expression}")]
    public class Switch : Statement
    {
        public List<SwitchSection> Sections { get; } = new List<SwitchSection>();

        public string Expression { get; set; }

        [JsonIgnore]
        public override List<Statement> Statements => this.Sections.SelectMany(s => s.Statements).ToList();
    }
}
