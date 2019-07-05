using Newtonsoft.Json;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Field {Type,nq} {Name,nq}")]
    public class FieldDescription : MemberDescription
    {
        public string Type { get; }

        public string Initializer { get; set; }

        [JsonIgnore]
        public bool HasInitializer => this.Initializer != null;

        public override MemberType MemberType => MemberType.Field;

        public FieldDescription(string type, string name)
            : base(name)
        {
            this.Type = type;
        }
    }
}