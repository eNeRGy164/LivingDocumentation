using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;

namespace LivingDocumentation
{
     public abstract class MemberDescription : IMemberable
    {
        public abstract MemberType MemberType { get; }

        public string Name { get; }

        [DefaultValue(Modifier.Private)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Modifier Modifiers { get; set; }

        [JsonIgnore]
        public bool IsInherited { get; internal set; } = false;

        public IHaveDocumentationComments DocumentationComments { get; set; }

        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
        [JsonConverter(typeof(ConcreteTypeConverter<List<AttributeDescription>>))]
        public List<IAttributeDescription> Attributes { get; } = new List<IAttributeDescription>();

        public MemberDescription(string name)
        {
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MemberDescription)) return false;

            var other = (MemberDescription)obj;
            return Equals(this.MemberType, other.MemberType) && string.Equals(this.Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (this.MemberType, this.Name).GetHashCode();
        }
    }
}
