using Newtonsoft.Json;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Property {Type} {Name}")]
    public class PropertyDescription : MemberDescription
    {
        public string Type { get; }

        public string Initializer { get; set; }
        
        [JsonIgnore]
        public bool HasInitializer => this.Initializer != null;

        public PropertyDescription(string type, string name)
            : base(MemberType.Property, name)
        {
            this.Type = type;
        }
    }
}
