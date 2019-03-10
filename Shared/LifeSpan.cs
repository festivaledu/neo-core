using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Neo.Core.Shared
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Lifespan
    {
        Custom,
        Volatile,
        Temporary,
        Permanent
    }
}
