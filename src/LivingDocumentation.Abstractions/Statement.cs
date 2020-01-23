using Newtonsoft.Json;
using System.Collections.Generic;

namespace LivingDocumentation
{
    public abstract class Statement
    {
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Arrays)]
        public virtual List<Statement> Statements { get; } = new List<Statement>();
    }
}
