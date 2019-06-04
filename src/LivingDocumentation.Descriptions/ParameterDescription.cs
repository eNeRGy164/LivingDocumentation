using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Parameter {Type} {Name}")]
    public class ParameterDescription : IParameterDescription
    {
        public string Type { get; }

        public string Name { get; }

        public bool HasDefaultValue { get; set; }

        public ParameterDescription(string type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
    }
}
