using System.Collections.Generic;
using System.Diagnostics;

namespace roslyn_uml
{
    [DebuggerDisplay("Property {Type} {Name}")]
    public class PropertyDescription : MemberDescription
    {
        public string Type { get; }
        public bool IsOptional { get; internal set; }

        public PropertyDescription(string type, string name)
            : base(MemberType.Property, name)
        {
            this.Type = type;
        }
    }
}
