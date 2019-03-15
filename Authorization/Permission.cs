using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Neo.Core.Authorization
{
    /// <summary>
    ///     Specifies whether a permission is granted.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Permission
    {
        /// <summary>
        ///     The permission is denied.
        /// </summary>
        Deny,
        /// <summary>
        ///     The permission is granted.
        /// </summary>
        Allow,
        /// <summary>
        ///     The permission will be inherited.
        /// </summary>
        Inherit
    }
}
