using System.Collections.Generic;

namespace roslyn_uml
{
    public abstract class MemberDescription : IHaveModifiers
    {
        public string Name { get; }
        public MemberType MemberType { get; }
        public List<string> Modifiers { get; } = new List<string>();
        public bool IsInherited { get; set; } = false;

        public MemberDescription(MemberType memberType, string name)
        {
            this.MemberType = memberType;
            this.Name = name;
        }

        public bool IsStatic => this.Modifiers.Contains("static");
        public bool IsPublic => this.Modifiers.Contains("public");
        public bool IsInternal => this.Modifiers.Contains("internal");
        public bool IsProtected => this.Modifiers.Contains("protected");
        public bool IsPrivate => this.Modifiers.Contains("private") || !this.IsPublic && !this.IsInternal && !this.IsProtected;

    }
}