using System;
using System.Collections.Generic;
using Neo.Core.Authorization;

namespace Neo.Core.Shared
{
    public class Member : User
    {
        // TODO
        public Account Account { get; set; }

        public new Dictionary<string, object> Attributes {
            get => Account.Attributes;
            set => Account.Attributes = value;
        }

        public List<Group> Groups => Account.Groups;

        public new Identity Identity {
            get => Account.Identity;
            set => Account.Identity = value;
        }

        public new Guid InternalId => Account.InternalId;

        public new Dictionary<string, Permission> Permissions {
            get => Account.Permissions;
            set => Account.Permissions = value;
        }
    }
}
