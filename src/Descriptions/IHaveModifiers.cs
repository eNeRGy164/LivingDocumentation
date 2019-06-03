using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace roslyn_uml
{
    public interface IHaveModifiers
    {
        List<string> Modifiers { get; }

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