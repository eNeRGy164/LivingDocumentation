using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace LivingDocumentation
{
    [DebuggerDisplay("Method {ReturnType,nq} {Name,nq}")]
    public class MethodDescription : MemberDescription, IHaveAMethodBody
    {
        [DefaultValue("void")]
        public string ReturnType { get; }

        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.None)]
        [JsonConverter(typeof(ConcreteTypeConverter<List<ParameterDescription>>))]
        public List<IParameterDescription> Parameters { get; } = new List<IParameterDescription>();

        public List<Statement> Statements { get; } = new List<Statement>();

        public override MemberType MemberType => MemberType.Method;
        
        public MethodDescription(string returnType, string name)
                : base(name)
        {
            this.ReturnType = returnType;
        }
    }
}
