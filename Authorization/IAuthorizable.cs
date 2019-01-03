using System.Collections.Generic;

namespace Neo.Core.Authorization
{
    /// <summary>
    ///     Represents an authorizable element.
    /// </summary>
    public interface IAuthorizable
    {
        /// <summary>
        ///     The permissions given to this <see cref="IAuthorizable"/>.
        /// </summary>
        Dictionary<string, Permission> Permissions { get; set; }
    }
}
