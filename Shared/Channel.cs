using System;
using System.Collections.Generic;
using Neo.Core.Attribution;
using Neo.Core.Authorization;

namespace Neo.Core.Shared
{
    public class Channel : IAttributable
    {
        public List<Guid> ActiveMemberIds { get; set; } = new List<Guid>();

        public List<User> ActiveMembers => Pool.Server.Users.FindAll(u => ActiveMemberIds.Contains(u.InternalId));

        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        public string Id { get; set; }

        public Guid InternalId { get; set; } = Guid.NewGuid();

        public List<Guid> MemberIds { get; set; } = new List<Guid>();

        public Dictionary<Guid, Dictionary<string, Permission>> MemberPermissions { get; set; } = new Dictionary<Guid, Dictionary<string, Permission>>();
        
        public List<User> Members => Pool.Server.Users.FindAll(u => MemberIds.Contains(u.InternalId));

        public string Name { get; set; }
    }
}
