using System;
using System.Collections.Generic;
using System.Linq;

namespace Neo.Core.Shared
{
    public class Member : User
    {
        // TODO
        public Account Account { get; set; }

        public List<Group> Groups => Pool.Server.Groups.FindAll(g => g.IsMember(this)).OrderBy(g => g.SortValue).ToList();

        public new Guid InternalId => Account.InternalId;
    }
}
