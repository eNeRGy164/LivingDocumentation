using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LivingDocumentation
{
    [DebuggerDisplay("{Type} {Name} ({Namespace})")]
    public class TypeDescription : IHaveModifiers
    {
        public TypeDescription(TypeType type, string fullName)
        {
            this.Type = type;
            this.FullName = fullName ?? string.Empty;
        }

        public TypeType Type { get; }

        public string FullName { get; }

        public string Documentation { get; set; }

        public List<string> BaseTypes { get; } = new List<string>();

        public List<string> Modifiers { get; } = new List<string>();

        public List<AttributeDescription> Attributes { get; } = new List<AttributeDescription>();

        [JsonProperty]
        private List<MemberDescription> AllMembers { get; } = new List<MemberDescription>();

        [JsonIgnore]
        public string Name => this.FullName.Substring(Math.Max(0, this.FullName.LastIndexOf('.'))).Trim('.');

        [JsonIgnore]
        public string Namespace => this.FullName.Substring(0, Math.Max(this.FullName.LastIndexOf('.'), 0)).Trim('.');

        [JsonIgnore]
        private IEnumerable<MemberDescription> Members => this.AllMembers.Where(m => !m.IsInherited);

        [JsonIgnore]
        private IEnumerable<MemberDescription> InheritedMembers => this.AllMembers.Where(m => m.IsInherited);

        [JsonIgnore]
        public IReadOnlyList<ConstructorDescription> Constructors => this.Members.OfType<ConstructorDescription>().ToList();

        [JsonIgnore]
        public IReadOnlyList<PropertyDescription> Properties => this.Members.OfType<PropertyDescription>().ToList();

        [JsonIgnore]
        public IReadOnlyList<MethodDescription> Methods => this.Members.OfType<MethodDescription>().ToList();

        [JsonIgnore]
        public IReadOnlyList<FieldDescription> Fields => this.Members.OfType<FieldDescription>().ToList();

        [JsonIgnore]
        public IReadOnlyList<EnumMemberDescription> EnumMembers => this.Members.OfType<EnumMemberDescription>().ToList();

        public void AddMember(MemberDescription member)
        {
            this.AllMembers.Add(member);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TypeDescription)) return false;

            var other = (TypeDescription)obj;
            return string.Equals(this.FullName, other.FullName, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return this.FullName.GetHashCode();
        }
    }
}
