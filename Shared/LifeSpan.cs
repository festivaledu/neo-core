using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Neo.Core.Shared
{
    /// <summary>
    ///     Specifies the type of a <see cref="Channel"/>.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Lifespan
    {
        /// <summary>
        ///     The <see cref="Channel"/> will be deleted automatically on a given date.
        /// </summary>
        Custom,
        /// <summary>
        ///     The <see cref="Channel"/> will be deleted automatically when all members are gone.
        /// </summary>
        Volatile,
        /// <summary>
        ///     The <see cref="Channel"/> will be deleted automatically when the server stops.
        /// </summary>
        Temporary,
        /// <summary>
        ///     The <see cref="Channel"/> will persist and has to be deleted manually.
        /// </summary>
        Permanent
    }
}
