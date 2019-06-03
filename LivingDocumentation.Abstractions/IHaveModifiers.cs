using Newtonsoft.Json;
using System.Collections.Generic;

namespace LivingDocumentation
{
    public interface IHaveModifiers
    {
        List<string> Modifiers { get; }

        [JsonIgnore]
        bool IsStatic { get; }

        [JsonIgnore]
        bool IsPublic { get; }

        [JsonIgnore]
        bool IsInternal { get; }

        [JsonIgnore]
        bool IsPrivate { get; }

        [JsonIgnore]
        bool IsProtected { get; }
    }
}