using Newtonsoft.Json;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Field {Type} {Name}")]
    public class FieldDescription : MemberDescription
    {
        public string Type { get; }

        public string Initializer { get; set; }

        [JsonIgnore]
        public bool HasInitializer => this.Initializer != null;

        public FieldDescription(string type, string name)
            : base(MemberType.Field, name)
        {
            this.Type = type;
        }
    }
}