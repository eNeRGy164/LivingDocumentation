using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace roslyn_uml
{
    [DebuggerDisplay("Property {Type} {Name}")]
    public class PropertyDescription : MemberDescription
    {
        public string Type { get; }

        public string Initializer { get; internal set; }
        
        [JsonIgnore]
        public bool HasInitializer => this.Initializer != null;

        public PropertyDescription(string type, string name)
            : base(MemberType.Property, name)
        {
            this.Type = type;
        }
    }
}
