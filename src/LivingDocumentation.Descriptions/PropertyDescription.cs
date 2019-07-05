using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Property {Type,nq} {Name,nq}")]
    public class PropertyDescription : MemberDescription
    {
        public string Type { get; }

        public string Initializer { get; set; }
        
        [JsonIgnore]
        public bool HasInitializer => this.Initializer != null;

        public override MemberType MemberType => MemberType.Property;

        public PropertyDescription(string type, string name)
            : base(name)
        {
            this.Type = type;
        }
    }
}
