using System;
using System.Collections.Generic;
using System.Linq;
using Neo.Core.Attribution;
using Neo.Core.Authorization;

namespace Neo.Core.Shared
{
    public class Group : IAttributable, IAuthorizable
    {
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        public string Id { get; set; }

        public Guid InternalId { get; set; } = Guid.NewGuid();

        public List<Guid> MemberIds { get; set; } = new List<Guid>();

        public List<Account> Members => MemberIds.Select(m => Pool.Server.Accounts.Find(a => a.InternalId == m)).ToList();

        public string Name { get; set; }

        public Dictionary<string, Permission> Permissions { get; set; } = new Dictionary<string, Permission>();

        public int SortValue { get; set; }
    }
}
