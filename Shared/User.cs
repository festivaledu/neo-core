using System;
using System.Collections.Generic;
using Neo.Core.Attribution;
using Neo.Core.Authorization;
using Neo.Core.Networking;

namespace Neo.Core.Shared
{
    public abstract class User : IAttributable, IAuthorizable
    {
        public Channel ActiveChannel => Pool.Server.Channels.Find(c => c.ActiveMemberIds.Contains(InternalId));

        public virtual Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();
        
        public List<Channel> Channels => Pool.Server.Channels.FindAll(c => c.MemberIds.Contains(InternalId));
        
        // TODO
        public Client Client { get; set; }
        
        public virtual Identity Identity { get; set; }

        public virtual Guid InternalId { get; set; } = Guid.NewGuid();
        
        public virtual Dictionary<string, Permission> Permissions { get; set; } = new Dictionary<string, Permission>();
    }
}
