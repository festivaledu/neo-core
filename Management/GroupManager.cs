using System.Linq;
using Neo.Core.Authorization;
using Neo.Core.Communication;
using Neo.Core.Communication.Packages;
using Neo.Core.Networking;
using Neo.Core.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Neo.Core.Management
{
    /// <summary>
    ///     Provides methods to manage <see cref="Group"/>s.
    /// </summary>
    public static class GroupManager
    {
        /// <summary>
        ///     Adds a <see cref="Guest"/> to the guest <see cref="Group"/>.
        /// </summary>
        /// <param name="guest">The <see cref="Guest"/> to add.</param>
        public static void AddGuestToGroup(Guest guest) {
            GetGuestGroup().MemberIds.Add(guest.InternalId);

            RefreshGroups();
        }

        /// <summary>
        ///     Adds a <see cref="Member"/> to a <see cref="Channel"/>.
        /// </summary>
        /// <param name="member">The <see cref="Member"/> to add.</param>
        /// <param name="group">The <see cref="Group"/> to add the <see cref="Member"/> to.</param>
        public static void AddMemberToGroup(Member member, Group group) {
            group.MemberIds.Add(member.InternalId);
            Pool.Server.DataProvider.Save();

            RefreshGroups();
        }
        
        // TODO: Add docs
        public static GroupActionResult CreateGroup(Group group, User creator) {
            if (!creator.IsAuthorized("neo.group.create")) {
                return GroupActionResult.NotAllowed;
            }

            if (Pool.Server.Groups.Any(_ => _.Id == group.Id)) {
                return GroupActionResult.IdInUse;
            }
            
            Pool.Server.Groups.Add(group);
            RefreshGroups();

            return GroupActionResult.Success;
        }

        // TODO: Add docs
        public static GroupActionResult DeleteGroup(Group group, User deletor) {
            if (!deletor.IsAuthorized("neo.group.delete")) {
                return GroupActionResult.NotAllowed;
            }

            var members = group.Members.Select(_ => _.InternalId);

            Pool.Server.Groups.Remove(group);

            members.Select(member => Pool.Server.Accounts.Find(account => account.InternalId == member)).ToList().ForEach(account => {
                if (account.Groups.Count == 0) {
                    GetUserGroup().MemberIds.Add(account.InternalId);
                }
            });

            RefreshGroups();

            return GroupActionResult.Success;
        }

        /// <summary>
        ///     Returns the admin <see cref="Group"/>.
        /// </summary>
        /// <returns>Returns the admin <see cref="Group"/>.</returns>
        public static Group GetAdminGroup() {
            return Pool.Server.Groups.Find(_ => _.Attributes.ContainsKey("neo.grouptype") && _.Attributes["neo.grouptype"].ToString() == "admin");
        }

        /// <summary>
        ///     Returns the guest <see cref="Group"/>.
        /// </summary>
        /// <returns>Returns the guest <see cref="Group"/>.</returns>
        public static Group GetGuestGroup() {
            return Pool.Server.Groups.Find(_ => _.Attributes.ContainsKey("neo.grouptype") && _.Attributes["neo.grouptype"].ToString() == "guest");
        }

        /// <summary>
        ///     Returns the default <see cref="Group"/>.
        /// </summary>
        /// <returns>Returns the default <see cref="Group"/>.</returns>
        public static Group GetUserGroup() {
            return Pool.Server.Groups.Find(_ => _.Attributes.ContainsKey("neo.grouptype") && _.Attributes["neo.grouptype"].ToString() == "user");
        }

        /// <summary>
        ///     Updates the list of <see cref="Group"/>s for each connected <see cref="User"/>.
        /// </summary>
        public static void RefreshGroups() {
            Target.All.SendPackage(new Package(PackageType.GroupListUpdate, Pool.Server.Groups));

            Pool.Server.Users.ForEach(_ => _.ToTarget().SendPackage(new Package(PackageType.GrantedPermissionsUpdate, _.GetAllPermissons())));
        }

        /// <summary>
        ///     Removes a <see cref="Guest"/> from the guest <see cref="Group"/>.
        /// </summary>
        /// <param name="guest">The <see cref="Guest"/> to remove.</param>
        public static void RemoveGuestFromGroup(Guest guest) {
            GetGuestGroup().MemberIds.Remove(guest.InternalId);

            RefreshGroups();
        }

        /// <summary>
        ///     Removes a <see cref="Member"/> from a <see cref="Group"/>.
        /// </summary>
        /// <param name="member">The <see cref="Member"/> to remove.</param>
        /// <param name="group">The <see cref="Group"/> to remove the <see cref="Member"/> from.</param>
        public static void RemoveMemberFromGroup(Member member, Group group) {
            group.MemberIds.Remove(member.InternalId);
            Pool.Server.DataProvider.Save();

            RefreshGroups();
        }
    }

    // TODO: Add docs
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GroupActionResult
    {
        Success,
        NotAllowed,
        IdInUse,
    }
}
