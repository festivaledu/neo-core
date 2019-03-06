using Neo.Core.Communication;
using Neo.Core.Networking;
using Neo.Core.Shared;

namespace Neo.Core.Management
{
    public static class GroupManager
    {
        public static void AddGuestToGroup(Guest guest) {
            Pool.Server.Groups[0].MemberIds.Add(guest.InternalId);
            Pool.Server.DataProvider.Save();
            RefreshGroups();
        }

        public static void AddMemberToGroup(Member member, Group group) {
            group.MemberIds.Add(member.InternalId);
            Pool.Server.DataProvider.Save();
            RefreshGroups();
        }

        public static void RefreshGroups() {
            Target.All.SendPackageTo(new Package(PackageType.GroupListUpdate, Pool.Server.Groups));
        }

        public static void RemoveMemberFromGroup(Member member, Group group) {
            group.MemberIds.Remove(member.InternalId);
            Pool.Server.DataProvider.Save();
            RefreshGroups();
        }
    }
}
