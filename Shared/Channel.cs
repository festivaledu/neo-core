using System;
using System.Collections.Generic;
using Neo.Core.Attribution;
using Neo.Core.Authorization;

namespace Neo.Core.Shared
{
    public class Channel : IAttributable
    {
        public Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        public Guid InternalId { get; private set; }

        public List<Guid> MemberIds { get; set; } = new List<Guid>();

        public Dictionary<string, Dictionary<string, Permission>> MemberPermissions { get; set; } = new Dictionary<string, Dictionary<string, Permission>>();

        // TODO
        public List<Member> Members { get; }

        public bool IsMember(Member member) {
            return MemberIds.Contains(member.InternalId);
        }
    }
}
