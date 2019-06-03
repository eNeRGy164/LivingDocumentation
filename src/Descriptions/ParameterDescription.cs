using System;
using System.Diagnostics;

namespace roslyn_uml
{
    [DebuggerDisplay("Parameter {Type} {Name}")]
    public class ParameterDescription
    {
        public string Type { get; }

        public string Name { get; }

        public bool HasDefaultValue { get; internal set; }

        public ParameterDescription(string type, string name)
        {
            this.Type = type;
            this.Name = name;
        }
    }
}
