using Newtonsoft.Json;
using System.ComponentModel;

namespace LivingDocumentation
{
     public abstract class MemberDescription : IMemberable
    {
        public abstract MemberType MemberType { get; }

        public string Name { get; }

        [DefaultValue(Modifier.Private)]
        public Modifier Modifiers { get; set; }

        public bool IsInherited { get; internal set; } = false;

        public string Documentation { get; set; }

        public MemberDescription(string name)
        {
            this.Name = name;
        }
    }
}