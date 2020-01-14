using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("AttributeArgument {Name} {Type} {Value}")]
    public class AttributeArgumentDescription : IAttributeArgumentDescription
    {
        public string Name { get; }

        public string Type { get; }

        public string Value { get; }

        public AttributeArgumentDescription(string name, string type, string value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }
    }
}
