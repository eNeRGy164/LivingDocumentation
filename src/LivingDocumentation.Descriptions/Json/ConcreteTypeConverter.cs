using Newtonsoft.Json;
using System;

namespace LivingDocumentation
{
    internal class ConcreteTypeConverter<TConcrete> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<TConcrete>(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.TypeNameHandling = TypeNameHandling.None;

            serializer.Serialize(writer, value);
        }
    }
}
