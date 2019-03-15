using System;
using System.Collections.Generic;
using Neo.Core.Authorization;

namespace Neo.Core.Shared
{
    /// <summary>
    ///     Represents a registered <see cref="User"/>.
    /// </summary>
    public class Member : User
    {
        /// <summary>
        ///     The <see cref="Neo.Core.Shared.Account"/> associated with this <see cref="Member"/>.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        ///     The attributes assigned to this <see cref="Member"/>.
        /// </summary>
        public override Dictionary<string, object> Attributes {
            get => Account.Attributes;
            set => Account.Attributes = value;
        }

        /// <summary>
        ///     Contains all <see cref="Group"/>s this <see cref="Member"/> is a member of.
        /// </summary>
        public List<Group> Groups => Account.Groups;

        /// <summary>
        ///     The <see cref="Neo.Core.Shared.Identity"/> of this <see cref="Member"/>.
        /// </summary>
        public override Identity Identity {
            get => Account.Identity;
            set => Account.Identity = value;
        }

        /// <summary>
        ///     The unique id used to identify this <see cref="Member"/>.
        /// </summary>
        public override Guid InternalId => Account.InternalId;

        /// <summary>
        ///     The permissions granted to this <see cref="Member"/>.
        /// </summary>
        public override Dictionary<string, Permission> Permissions {
            get => Account.Permissions;
            set => Account.Permissions = value;
        }
    }
}
