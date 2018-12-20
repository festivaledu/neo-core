using System;
using System.Collections.Generic;
using Neo.Core.Attribution;
using Neo.Core.Authorization;
using Neo.Core.Networking;

namespace Neo.Core.Shared
{
    public abstract class Member : IAttributable, IAuthorizable
    {
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        // TODO
        public List<Channel> Channels { get; }
        
        // TODO
        public Client Client { get; }
        
        public Identity Identity { get; set; }
        
        public Guid InternalId { get; private set; }
        
        public Dictionary<string, Permission> Permissions { get; set; } = new Dictionary<string, Permission>();
    }
}
