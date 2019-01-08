using System.Linq;
using Neo.Core.Authorization;
using Neo.Core.Shared;

namespace Neo.Core.Management
{
    public static class ChannelManager
    {
        private static void AddUserToChannel(User user, Channel channel) {
            channel.MemberIds.Add(user.InternalId);
            RefreshChannel(channel);
        }

        public static ChannelActionResult JoinChannel(this User user, Channel channel, string password = "") {
            if (!user.IsAuthorized("neo.channel.join.$")) {
                return ChannelActionResult.NotAllowed;
            }

            if (channel.BlacklistedGroupIds.Count > 0 || channel.BlacklistedUserIds.Count > 0) {
                if (user is Member member && channel.BlacklistedGroupIds.Any(member.Groups.Select(g => g.InternalId).Contains) || channel.BlacklistedUserIds.Contains(user.InternalId)) {
                    if (user.IsAuthorized("neo.channel.join.ignoreblacklist")) {
                        AddUserToChannel(user, channel);
                        return ChannelActionResult.Success;
                    }

                    return ChannelActionResult.Blacklisted;
                }
            }

            if (channel.Password != password) {
                if (user.IsAuthorized("neo.channel.join.ignorepassword")) {
                    AddUserToChannel(user, channel);
                    return ChannelActionResult.Success;
                }

                return ChannelActionResult.IncorrectPassword;
            }

            if (channel.MemberIds.Count >= channel.Limit) {
                if (user.IsAuthorized("neo.channel.join.ignorelimit")) {
                    AddUserToChannel(user, channel);
                    return ChannelActionResult.Success;
                }

                return ChannelActionResult.Full;
            }

            if (channel.WhitelistedGroupIds.Count > 0 || channel.WhitelistedUserIds.Count > 0) {
                if (user is Member member && !channel.WhitelistedGroupIds.Any(member.Groups.Select(g => g.InternalId).Contains) || !channel.WhitelistedUserIds.Contains(user.InternalId)) {
                    if (user.IsAuthorized("neo.channel.join.ignorewhitelist")) {
                        AddUserToChannel(user, channel);
                        return ChannelActionResult.Success;
                    }

                    return ChannelActionResult.NotWhitelisted;
                }
            }

            AddUserToChannel(user, channel);
            return ChannelActionResult.Success;
        }

        public static void MoveToChannel(this User user, Channel channel) {
            if (!channel.ActiveMemberIds.Contains(user.InternalId)) {
                var currentActiveChannel = user.ActiveChannel;

                if (currentActiveChannel != null) {
                    user.ActiveChannel.ActiveMemberIds.Remove(user.InternalId);
                    RefreshChannel(currentActiveChannel);
                }
                
                channel.ActiveMemberIds.Add(user.InternalId);
                RefreshChannel(channel);

                // TODO: Perform actual networking stuff to open new channel in the client
            }
        }

        public static ChannelActionResult OpenChannel(this User user, Channel channel, string password = "") {
            if (!channel.MemberIds.Contains(user.InternalId)) {
                var result = JoinChannel(user, channel, password);
                if (result != ChannelActionResult.Success) {
                    return result;
                }
            }
               
            MoveToChannel(user, channel);
            return ChannelActionResult.Success;
        }

        public static void RefreshChannel(Channel channel) {
            // TODO: Send channel data to members
        }
    }

    public enum ChannelActionResult
    {
        Success,
        NotAllowed,
        Blacklisted,
        IncorrectPassword,
        Full,
        NotWhitelisted
    }
}
