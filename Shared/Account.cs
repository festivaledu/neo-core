using System;
using System.Collections.Generic;
using System.Linq;
using Neo.Core.Attribution;
using Neo.Core.Authorization;
using Newtonsoft.Json;

namespace Neo.Core.Shared
{
    public class Account : IAttributable, IAuthorizable
    {
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        public string Email { get; set; }

        [JsonIgnore]
        public List<Group> Groups => Pool.Server.Groups.FindAll(g => g.MemberIds.Contains(InternalId)).OrderBy(g => g.SortValue).ToList();

        public Identity Identity { get; set; }

        public Guid InternalId { get; set; } = Guid.NewGuid();

        [JsonIgnore]
        public Member Member => (Member) Pool.Server.Users.Find(u => u.InternalId == InternalId);

        public byte[] Password { get; set; }

        public Dictionary<string, Permission> Permissions { get; set; } = new Dictionary<string, Permission>();
    }
}
