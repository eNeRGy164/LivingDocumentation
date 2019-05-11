using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace roslyn_uml
{

    [DebuggerDisplay("{Type} {Name} ({Namespace})")]
    public class TypeDescription : IHaveModifiers
    {
        public TypeDescription(TypeType type, string fullName)
        {
            this.Type = type;
            this.FullName = fullName;
        }

        public TypeType Type { get; }
        public string FullName { get; }
        public string Name => this.FullName.Substring(Math.Max(0, this.FullName.LastIndexOf('.'))).Trim('.');
        public string Namespace => this.FullName.Substring(0, this.FullName.LastIndexOf('.')).Trim('.');
        public List<string> Modifiers { get; } = new List<string>();
        public bool IsStatic => this.Modifiers.Contains("static");
        public bool IsPublic => this.Modifiers.Contains("public");
        public bool IsInternal => this.Modifiers.Contains("internal");
        public bool IsProtected => this.Modifiers.Contains("protected");
        public bool IsPrivate => !this.IsPublic && !this.IsInternal;
        public bool IsAbstract => this.Modifiers.Contains("abstract");
        public List<string> BaseTypes { get; } = new List<string>();
        private List<MemberDescription> AllMembers { get; } = new List<MemberDescription>();
        private IEnumerable<MemberDescription> Members => this.AllMembers.Where(m => !m.IsInherited);
        private IEnumerable<MemberDescription> InheritedMembers => this.AllMembers.Where(m => m.IsInherited);
        public IReadOnlyList<ConstructorDescription> Constructors => this.Members.OfType<ConstructorDescription>().ToList();
        public IReadOnlyList<PropertyDescription> Properties => this.Members.OfType<PropertyDescription>().ToList();
        public IReadOnlyList<MethodDescription> Methods => this.Members.OfType<MethodDescription>().ToList();
        public IReadOnlyList<FieldDescription> Fields => this.Members.OfType<FieldDescription>().ToList();

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
