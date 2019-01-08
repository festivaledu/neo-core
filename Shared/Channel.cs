using System;
using System.Collections.Generic;
using Neo.Core.Attribution;
using Neo.Core.Authorization;
using Newtonsoft.Json;

namespace Neo.Core.Shared
{
    public class Channel : IAttributable
    {
        public List<Guid> ActiveMemberIds { get; set; } = new List<Guid>();

        [JsonIgnore]
        public List<User> ActiveMembers => Pool.Server.Users.FindAll(u => ActiveMemberIds.Contains(u.InternalId));

        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        public List<Guid> BlacklistedGroupIds { get; set; } = new List<Guid>();

        public List<Guid> BlacklistedUserIds { get; set; } = new List<Guid>();

        public string Id { get; set; }

        public Guid InternalId { get; set; } = Guid.NewGuid();

        public int Limit { get; set; }

        public List<Guid> MemberIds { get; set; } = new List<Guid>();

        public Dictionary<Guid, Dictionary<string, Permission>> MemberPermissions { get; set; } = new Dictionary<Guid, Dictionary<string, Permission>>();

        [JsonIgnore]
        public List<User> Members => Pool.Server.Users.FindAll(u => MemberIds.Contains(u.InternalId));

        public string Name { get; set; }

        public string Password { get; set; }

        public List<Guid> WhitelistedGroupIds { get; set; } = new List<Guid>();

        public List<Guid> WhitelistedUserIds { get; set; } = new List<Guid>();
    }
}
