using System;
using System.Linq;
using Neo.Core.Authorization;
using Neo.Core.Communication;
using Neo.Core.Communication.Packages;
using Neo.Core.Config;
using Neo.Core.Extensibility;
using Neo.Core.Networking;
using Neo.Core.Shared;

namespace Neo.Core.Management
{
    public static class ChannelManager
    {
        private static void AddUserToChannel(User user, Channel channel) {
            channel.MemberIds.Add(user.InternalId);

            // TODO: Inform about new member
            Pool.Server.SendPackageTo(new Target().AddMany(channel), new Package(PackageType.Message, MessagePackageContent.GetSystemMessage(user.Identity.Name + " ist dem Channel beigetreten.", channel.InternalId)));

            RefreshChannels();
        }

        public static void CloseChannel(User user) {
            if (user.ActiveChannel != null) {
                var channel = user.ActiveChannel;
                channel.ActiveMemberIds.Remove(user.InternalId);
                RefreshChannels();
            }
        }

        public static bool CreateChannel(this Plugin plugin, Member member, Channel channel) {
            if (Pool.Server.Channels.Any(c => c.Id == channel.Id)) {
                return false;
            }

            channel.Attributes.Add("neo.origin", plugin.InternalId);
            channel.Owner = member.InternalId;
            AddUserToChannel(member, channel);

            return true;
        }

        public static bool CreateChannel(this User user, Channel channel) {
            if (Pool.Server.Channels.Any(c => c.Id == channel.Id)) {
                return false;
            }

            // TODO: Check for rights

            channel.Attributes.Add("neo.origin", "neo.client");
            channel.Owner = user.InternalId;
            Pool.Server.Channels.Add(channel);

            RefreshChannels();

            AddUserToChannel(user, channel);
            MoveToChannel(user, channel);

            return true;
        }

        public static bool DeleteChannel(this Channel channel, User user) {
            // TODO: Check for rights
            RemoveChannel(channel);
            RefreshChannels();

            return true;
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

            if (!string.IsNullOrEmpty(channel.Password) && channel.Password != password) {
                if (user.IsAuthorized("neo.channel.join.ignorepassword")) {
                    AddUserToChannel(user, channel);
                    return ChannelActionResult.Success;
                }

                return ChannelActionResult.IncorrectPassword;
            }

            if (channel.MemberIds.Count >= channel.Limit && channel.Limit > -1) {
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

        public static void LeaveChannel(this User user, Channel channel) {
            channel.ActiveMemberIds.Remove(user.InternalId);
            channel.MemberIds.Remove(user.InternalId);
            RefreshChannels();
        }

        public static void MoveToChannel(this User user, Channel channel) {
            if (!channel.ActiveMemberIds.Contains(user.InternalId)) {
                var currentActiveChannel = user.ActiveChannel;

                if (currentActiveChannel != null) {
                    user.ActiveChannel.ActiveMemberIds.Remove(user.InternalId);
                }
                
                channel.ActiveMemberIds.Add(user.InternalId);
                RefreshChannels();

                // TODO: Perform actual networking stuff to open new channel in the client
                
                user.ToTarget().SendPackageTo(new Package(PackageType.EnterChannelResponse, channel));
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

        public static void RefreshChannels() {
            // TODO: Send channel data to members
            Pool.Server.SendPackageTo(Target.All, new Package(PackageType.ChannelListUpdate, Pool.Server.Channels));
        }

        public static void RemoveChannel(Channel channel) {
            foreach (var activeMember in channel.ActiveMembers) {
                MoveToChannel(activeMember, Pool.Server.Channels[0]);
            }
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
