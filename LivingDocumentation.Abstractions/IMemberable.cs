using Newtonsoft.Json;

namespace LivingDocumentation
{
    public interface IMemberable : IHaveModifiers
    {
        [JsonProperty(Order = 1)]
        MemberType MemberType { get; }

        [JsonProperty(Order = 2)]
        string Name { get; }

        string Documentation { get; }
    }
}