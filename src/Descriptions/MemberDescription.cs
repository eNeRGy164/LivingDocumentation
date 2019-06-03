using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace roslyn_uml
{
     public abstract class MemberDescription : IHaveModifiers
    {
        [JsonProperty(Order = 1)]
        public MemberType MemberType { get; }

        [JsonProperty(Order = 2)]
        public string Name { get; }
        
        public List<string> Modifiers { get; } = new List<string>();

        public bool IsInherited { get; internal set; } = false;

        public string Documentation { get; internal set; }

        public MemberDescription(MemberType memberType, string name)
        {
            this.MemberType = memberType;
            this.Name = name;
        }

        public bool IsStatic => this.Modifiers.Contains("static");
        public bool IsPublic => this.Modifiers.Contains("public");
        public bool IsInternal => this.Modifiers.Contains("internal");
        public bool IsProtected => this.Modifiers.Contains("protected");
        public bool IsPrivate => this.Modifiers.Contains("private") || !this.IsPublic && !this.IsInternal;
    }
}