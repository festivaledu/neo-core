using System;
using System.Collections.Generic;
using Neo.Core.Authorization;

namespace Neo.Core.Shared
{
    public class Member : User
    {
        public Account Account { get; set; }

        public override Dictionary<string, object> Attributes {
            get => Account.Attributes;
            set => Account.Attributes = value;
        }

        public List<Group> Groups => Account.Groups;

        public override Identity Identity {
            get => Account.Identity;
            set => Account.Identity = value;
        }

        public override Guid InternalId => Account.InternalId;

        public override Dictionary<string, Permission> Permissions {
            get => Account.Permissions;
            set => Account.Permissions = value;
        }
    }
}
