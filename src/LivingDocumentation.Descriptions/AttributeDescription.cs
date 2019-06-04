using System.Collections.Generic;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Attribute {Type} {Name}")]
    public class AttributeDescription
    {
        public string Type { get; }

        public string Name { get; }

        public List<AttributeArgumentDescription> Arguments { get; } = new List<AttributeArgumentDescription>();

        public AttributeDescription(string type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
    }
}
