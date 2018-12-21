using System;
using System.Collections.Generic;
using Neo.Core.Attribution;
using Neo.Core.Authorization;
using Neo.Core.Networking;

namespace Neo.Core.Shared
{
    public abstract class User : IAttributable, IAuthorizable
    {
        public Channel ActiveChannel { get; }

        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
        
        public List<Channel> Channels => Pool.Server.Channels.FindAll(c => c.IsMember(this));
        
        // TODO
        public Client Client { get; }
        
        public Identity Identity { get; set; }
        
        public Guid InternalId { get; private set; }

        public string Locale { get; set; }
        
        public Dictionary<string, Permission> Permissions { get; set; } = new Dictionary<string, Permission>();
    }
}
