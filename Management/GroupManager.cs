using Neo.Core.Communication;
using Neo.Core.Networking;
using Neo.Core.Shared;

namespace Neo.Core.Management
{
    public static class GroupManager
    {
        public static void AddGuestToGroup(Guest guest) {
            GetGuestGroup().MemberIds.Add(guest.InternalId);
            RefreshGroups();
        }

        public static void AddMemberToGroup(Member member, Group group) {
            group.MemberIds.Add(member.InternalId);
            Pool.Server.DataProvider.Save();
            RefreshGroups();
        }

        public static Group GetAdminGroup() {
            return Pool.Server.Groups.Find(g => g.Attributes.ContainsKey("neo.grouptype") && g.Attributes["neo.grouptype"].ToString() == "admin");
        }

        public static Group GetGuestGroup() {
            return Pool.Server.Groups.Find(g => g.Attributes.ContainsKey("neo.grouptype") && g.Attributes["neo.grouptype"].ToString() == "guest");
        }

        public static Group GetUserGroup() {
            return Pool.Server.Groups.Find(g => g.Attributes.ContainsKey("neo.grouptype") && g.Attributes["neo.grouptype"].ToString() == "user");
        }

        public static void RefreshGroups() {
            Target.All.SendPackageTo(new Package(PackageType.GroupListUpdate, Pool.Server.Groups));
        }

        public static void RemoveGuestFromGroup(Guest guest) {
            GetGuestGroup().MemberIds.Remove(guest.InternalId);
            RefreshGroups();
        }

        public static void RemoveMemberFromGroup(Member member, Group group) {
            group.MemberIds.Remove(member.InternalId);
            Pool.Server.DataProvider.Save();
            RefreshGroups();
        }
    }
}
