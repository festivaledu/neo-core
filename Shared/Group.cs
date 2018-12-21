using System;
using System.Collections.Generic;
using Neo.Core.Attribution;
using Neo.Core.Authorization;

namespace Neo.Core.Shared
{
    public class Group : IAttributable, IAuthorizable
    {
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        public Guid InternalId { get; private set; }

        public List<Guid> MemberIds { get; set; } = new List<Guid>();

        public List<Account> Members { get; }

        public string Name { get; set; }

        public Dictionary<string, Permission> Permissions { get; set; } = new Dictionary<string, Permission>();

        public int SortValue { get; set; }

        public bool IsMember(User account) {
            return MemberIds.Contains(account.InternalId);
        }
    }
}
