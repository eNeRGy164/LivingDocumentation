using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Constructor {Name,nq}")]
    public class ConstructorDescription : MemberDescription, IHaveAMethodBody
    {
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
        [JsonConverter(typeof(ConcreteTypeConverter<List<ParameterDescription>>))]
        public List<IParameterDescription> Parameters { get; } = new List<IParameterDescription>();

        public List<Statement> Statements { get; } = new List<Statement>();

        public override MemberType MemberType => MemberType.Constructor;

        public ConstructorDescription(string name)
            : base(name)
        {
        }
    }
}