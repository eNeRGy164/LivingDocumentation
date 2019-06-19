using System.Collections.Generic;

namespace LivingDocumentation
{
     public abstract class MemberDescription : IMemberable
    {
        public MemberType MemberType { get; }

        public string Name { get; }
        
        public List<string> Modifiers { get; } = new List<string>();

        public bool IsInherited { get; internal set; } = false;

        public string Documentation { get; set; }

        public MemberDescription(MemberType memberType, string name)
        {
            this.MemberType = memberType;
            this.Name = name;
        }
    }
}