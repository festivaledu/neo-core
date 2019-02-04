using Neo.Core.Shared;

namespace Neo.Core.Management
{
    public static class GroupManager
    {
        public static void AddMemberToGroup(Member member, Group group) {
            group.MemberIds.Add(member.InternalId);
        }
    }
}
