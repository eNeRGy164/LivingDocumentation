using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("{Type} {Name,nq} ({Namespace,nq})")]
    public class TypeDescription : IHaveModifiers
    {

        [JsonProperty(Order = 1, PropertyName = nameof(Fields))]
        private readonly List<FieldDescription> fields = new List<FieldDescription>();

        [JsonProperty(Order = 2, PropertyName = nameof(Constructors))]
        private readonly List<ConstructorDescription> constructors = new List<ConstructorDescription>();

        [JsonProperty(Order = 3, PropertyName = nameof(Properties))]
        private readonly List<PropertyDescription> properties = new List<PropertyDescription>();

        [JsonProperty(Order = 4, PropertyName = nameof(Methods))]
        private readonly List<MethodDescription> methods = new List<MethodDescription>();

        [JsonProperty(Order = 5, PropertyName = nameof(EnumMembers))]
        private readonly List<EnumMemberDescription> enumMembers = new List<EnumMemberDescription>();

        [JsonProperty(Order = 6, PropertyName = nameof(Events))]
        private readonly List<EventDescription> events = new List<EventDescription>();

        public TypeDescription(TypeType type, string fullName)
        {
            this.Type = type;
            this.FullName = fullName ?? string.Empty;
        }

        public TypeType Type { get; }

        public string FullName { get; }

        public DocumentationCommentsDescription DocumentationComments { get; set; }

        public List<string> BaseTypes { get; } = new List<string>();

        [DefaultValue(Modifier.Internal)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Modifier Modifiers { get; set; }

        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
        [JsonConverter(typeof(ConcreteTypeConverter<List<AttributeDescription>>))]
        public List<IAttributeDescription> Attributes { get; } = new List<IAttributeDescription>();

        [JsonIgnore]
        public string Name => this.FullName.Substring(Math.Min(this.FullName.LastIndexOf('.') + 1, this.FullName.Length)).ToString();

        [JsonIgnore]
        public string Namespace => this.FullName.Substring(0, Math.Max(this.FullName.LastIndexOf('.'), 0));

        [JsonIgnore]
        public IReadOnlyList<ConstructorDescription> Constructors => this.constructors;

        [JsonIgnore]
        public IReadOnlyList<PropertyDescription> Properties => this.properties;

        [JsonIgnore]
        public IReadOnlyList<MethodDescription> Methods => this.methods;

        [JsonIgnore]
        public IReadOnlyList<EventDescription> Events => this.events;

        [JsonIgnore]
        public IReadOnlyList<FieldDescription> Fields => this.fields;

        [JsonIgnore]
        public IReadOnlyList<EnumMemberDescription> EnumMembers => this.enumMembers;

        public void AddMember(MemberDescription member)
        {
            switch (member)
            {
                case ConstructorDescription c:
                    this.constructors.Add(c);
                    break;

                case FieldDescription f:
                    this.fields.Add(f);
                    break;

                case PropertyDescription p:
                    this.properties.Add(p);
                    break;

                case MethodDescription m:
                    this.methods.Add(m);
                    break;

                case EnumMemberDescription em:
                    this.enumMembers.Add(em);
                    break;

                case EventDescription e:
                    this.events.Add(e);
                    break;

                default:
                    throw new NotSupportedException($"Unable to add {member.GetType()} as member");
            }
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
