using System;
using Newtonsoft.Json;

namespace LivingDocumentation
{
    public static class JsonDefaults
    {
        public static JsonSerializerSettings SerializerSettings()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new SkipEmptyCollectionsContractResolver(),
                TypeNameHandling = TypeNameHandling.Auto
            };

            return serializerSettings;
        }

        public static JsonSerializerSettings DeserializerSettings()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                ContractResolver = new SkipEmptyCollectionsContractResolver(),
                TypeNameHandling = TypeNameHandling.Auto
            };

            return serializerSettings;
        }
    }
}
