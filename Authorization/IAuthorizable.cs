using System.Collections.Generic;

namespace Neo.Core.Authorization
{
    public interface IAuthorizable
    {
        Dictionary<string, Permission> Permissions { get; set; }
    }
}
