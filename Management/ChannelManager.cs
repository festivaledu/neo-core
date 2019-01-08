using System.Linq;
using Neo.Core.Authorization;
using Neo.Core.Shared;

namespace Neo.Core.Management
{
    public static class ChannelManager
    {
        public static ChannelActionResult JoinChannel(this User user, Channel channel, string password = "") {
            if (!user.IsAuthorized("neo.channel.join.$")) {
                return ChannelActionResult.NotAllowed;
            }

            if (channel.BlacklistedGroupIds.Count > 0 || channel.BlacklistedUserIds.Count > 0) {
                if (user is Member member && channel.BlacklistedGroupIds.Any(member.Groups.Select(g => g.InternalId).Contains) || channel.BlacklistedUserIds.Contains(user.InternalId)) {
                    if (user.IsAuthorized("neo.channel.join.ignoreblacklist")) {
                        // TODO: Add to channel
                        return ChannelActionResult.Success;
                    }

                    return ChannelActionResult.NotAllowed;
                }
            }

            if (channel.Password != password) {
                if (user.IsAuthorized("neo.channel.join.ignorepassword")) {
                    // TODO: Add to channel
                    return ChannelActionResult.Success;
                }

                return ChannelActionResult.NotAllowed;
            }

            if (channel.MemberIds.Count >= channel.Limit) {
                if (user.IsAuthorized("neo.channel.join.ignorelimit")) {
                    // TODO: Add to channel
                    return ChannelActionResult.Success;
                }

                return ChannelActionResult.NotAllowed;
            }

            if (channel.WhitelistedGroupIds.Count > 0 || channel.WhitelistedUserIds.Count > 0) {
                if (user.IsAuthorized("neo.channel.join.ignorewhitelist")) {
                    // TODO: Add to channel
                    return ChannelActionResult.Success;
                }

                return ChannelActionResult.NotAllowed;
            }

            // TODO: Add to channel
            return ChannelActionResult.Success;
        }
    }

    public enum ChannelActionResult
    {
        NotAllowed,
        Success
    }
}
