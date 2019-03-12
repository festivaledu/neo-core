using System;
using System.Collections.Generic;
using System.Linq;
using Neo.Core.Authorization;
using Neo.Core.Communication;
using Neo.Core.Communication.Packages;
using Neo.Core.Config;
using Neo.Core.Extensibility;
using Neo.Core.Extensibility.Events;
using Neo.Core.Networking;
using Neo.Core.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Neo.Core.Management
{
    /// <summary>
    ///     Provides methods to manage <see cref="Channel"/>s.
    /// </summary>
    public static class ChannelManager
    {
        /// <summary>
        ///     Adds a <see cref="User"/> to a <see cref="Channel"/> and informs all other channel members.
        /// </summary>
        /// <param name="user">The <see cref="User"/> to add.</param>
        /// <param name="channel">The <see cref="Channel"/> to add the <see cref="User"/> to.</param>
        private static void AddUserToChannel(User user, Channel channel) {
            channel.MemberIds.Add(user.InternalId);

            var message = MessagePackageContent.GetSystemMessage(user.Identity.Name + " ist dem Channel beigetreten.", channel.InternalId);
            new Target().AddMany(channel).SendPackage(new Package(PackageType.Message, message));

            if (ConfigManager.Instance.Values.SaveSystemMessages) {
                channel.SaveMessage(message);
            }

            RefreshChannels();
        }

        [Obsolete]
        public static void CloseChannel(User user) {
            if (user.ActiveChannel != null) {
                var channel = user.ActiveChannel;
                channel.ActiveMemberIds.Remove(user.InternalId);
                RefreshChannels();
            }
        }

        /// <summary>
        ///     Creates a new <see cref="Channel"/> for a <see cref="Plugin"/>.
        /// </summary>
        /// <param name="plugin">The <see cref="Plugin"/> that creates the <see cref="Channel"/>.</param>
        /// <param name="member">The <see cref="Member"/> that owns the <see cref="Channel"/>.</param>
        /// <param name="channel">The <see cref="Channel"/> to create.</param>
        /// <returns>Returns <c>false</c> if a channel with the given id already exists. Otherwise <c>true</c>.</returns>
        public static bool CreateChannel(this Plugin plugin, Member member, Channel channel) {
            if (Pool.Server.Channels.Any(_ => _.Id == channel.Id)) {
                return false;
            }

            channel.Attributes.Add("neo.origin", plugin.Namespace);
            channel.Owner = member.InternalId;
            Pool.Server.Channels.Add(channel);

            RefreshChannels();

            return true;
        }

        /// <summary>
        ///     Creates a new <see cref="Channel"/> for a <see cref="User"/>.
        /// </summary>
        /// <param name="user">The <see cref="User"/> who creates the <see cref="Channel"/>.</param>
        /// <param name="channel">The <see cref="Channel"/> to create.</param>
        /// <returns>Returns <c>false</c> if a channel with the given id already exists or the user is not authorized to create channels. Otherwise <c>true</c>.</returns>
        public static bool CreateChannel(this User user, Channel channel) {
            if (Pool.Server.Channels.Any(_ => _.Id == channel.Id)) {
                return false;
            }

            if (!user.IsAuthorized("neo.channel.create")) {
                return false;
            }

            channel.Attributes.Add("neo.origin", "neo.client");
            channel.Owner = user.InternalId;
            Pool.Server.Channels.Add(channel);

            RefreshChannels();

            AddUserToChannel(user, channel);
            MoveToChannel(user, channel);

            return true;
        }

        /// <summary>
        ///     Deletes a <see cref="Channel"/> for a <see cref="User"/>.
        /// </summary>
        /// <param name="channel">The <see cref="Channel"/> to delete.</param>
        /// <param name="user">The <see cref="User"/> who deletes the <see cref="Channel"/>.</param>
        /// <returns>Returns <c>false</c> if the user is not authorized to delete channels. Otherwise <c>true</c>.</returns>
        public static bool DeleteChannel(this Channel channel, User user) {
            if (!user.IsAuthorized("neo.channel.delete")) {
                return false;
            }

            RemoveChannel(channel);
            RefreshChannels();

            return true;
        }

        /// <summary>
        ///     Returns the default <see cref="Channel"/>.
        /// </summary>
        /// <returns>Returns the default <see cref="Channel"/>.</returns>
        public static Channel GetMainChannel() {
            return Pool.Server.Channels.Find(_ => _.Attributes.ContainsKey("neo.channeltype") && _.Attributes["neo.channeltype"].ToString() == "main");
        }

        // TODO: Add docs
        public static ChannelActionResult JoinChannel(this User user, Channel channel, string password = "") {
            if (!user.IsAuthorized("neo.channel.join.$")) {
                return ChannelActionResult.NotAllowed;
            }

            //if (channel.BlacklistedGroupIds.Count > 0 || channel.BlacklistedUserIds.Count > 0) {
            //    if (user is Member member && channel.BlacklistedGroupIds.Any(member.Groups.Select(g => g.InternalId).Contains) || channel.BlacklistedUserIds.Contains(user.InternalId)) {
            //        if (user.IsAuthorized("neo.channel.join.ignoreblacklist")) {
            //            AddUserToChannel(user, channel);
            //            return ChannelActionResult.Success;
            //        }

            //        return ChannelActionResult.Blacklisted;
            //    }
            //}

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

            //if (channel.WhitelistedGroupIds.Count > 0 || channel.WhitelistedUserIds.Count > 0) {
            //    if (user is Member member && !channel.WhitelistedGroupIds.Any(member.Groups.Select(g => g.InternalId).Contains) || !channel.WhitelistedUserIds.Contains(user.InternalId)) {
            //        if (user.IsAuthorized("neo.channel.join.ignorewhitelist")) {
            //            AddUserToChannel(user, channel);
            //            return ChannelActionResult.Success;
            //        }

            //        return ChannelActionResult.NotWhitelisted;
            //    }
            //}

            AddUserToChannel(user, channel);
            return ChannelActionResult.Success;
        }

        // TODO: Add docs
        public static void LeaveChannel(this User user, Channel channel) {
            var beforeChannelLeaveEvent = new Before<LeaveElementEventArgs<Channel>>(new LeaveElementEventArgs<Channel>(user, channel));
            EventService.RaiseEvent(EventType.BeforeChannelLeave, beforeChannelLeaveEvent);

            if (!beforeChannelLeaveEvent.Cancel) {
                channel.ActiveMemberIds.Remove(user.InternalId);
                channel.MemberIds.Remove(user.InternalId);

                RefreshChannels();

                EventService.RaiseEvent(EventType.ChannelLeft, new LeaveElementEventArgs<Channel>(user, channel));
            }
        }

        /// <summary>
        ///     Moves a <see cref="User"/> to a <see cref="Channel"/>, effectively setting the channel to the users active channel.
        /// </summary>
        /// <param name="user">The <see cref="User"/> to move.</param>
        /// <param name="channel">The <see cref="Channel"/> to move the <see cref="User"/> to.</param>
        /// <param name="noEvents">Whether this action should trigger events or not.</param>
        public static void MoveToChannel(this User user, Channel channel, bool noEvents = false) {
            if (!channel.ActiveMemberIds.Contains(user.InternalId)) {
                var beforeChannelJoinEvent = new Before<JoinElementEventArgs<Channel>>(new JoinElementEventArgs<Channel>(user, channel));

                if (!noEvents) {
                    EventService.RaiseEvent(EventType.BeforeChannelJoin, beforeChannelJoinEvent);
                }

                if (!beforeChannelJoinEvent.Cancel) {
                    var currentActiveChannel = user.ActiveChannel;

                    if (currentActiveChannel != null) {
                        // user.LeaveChannel(user.ActiveChannel);
                        user.ActiveChannel.ActiveMemberIds.Remove(user.InternalId);
                    }
                    
                    channel.ActiveMemberIds.Add(user.InternalId);

                    // Reference issue solution
                    var index = Pool.Server.Channels.FindIndex(_ => _.InternalId.Equals(channel.InternalId));
                    Pool.Server.Channels[index] = channel;

                    RefreshChannels();

                    if (!noEvents) {
                        EventService.RaiseEvent(EventType.ChannelJoined, new JoinElementEventArgs<Channel>(user, channel));
                    }

                    user.ToTarget().SendPackage(new Package(PackageType.EnterChannelResponse, new EnterChannelResponsePackageContent(ChannelActionResult.Success, channel)));
                }
            }
        }

        /// <summary>
        ///     Opens a <see cref="Channel"/> for a <see cref="User"/>. Performs a join if the user is not member of the channel already and then moves the user into the channel.
        /// </summary>
        /// <param name="user">The <see cref="User"/> to open the <see cref="Channel"/> for.</param>
        /// <param name="channel">The <see cref="Channel"/> to open for the <see cref="User"/>.</param>
        /// <param name="password">The password entered by the client.</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Updates the list of <see cref="Channel"/>s for each connected <see cref="User"/>.
        /// </summary>
        public static void RefreshChannels() {
            var channels = JsonConvert.DeserializeObject<List<Channel>>(JsonConvert.SerializeObject(Pool.Server.Channels));
            channels.ForEach(_ => _.Password = !string.IsNullOrEmpty(_.Password) ? "true" : null);

            Target.All.SendPackage(new Package(PackageType.ChannelListUpdate, channels));
        }

        /// <summary>
        ///     Removes a <see cref="Channel"/> and moves all remaining members to the default channel.
        /// </summary>
        /// <param name="channel">The <see cref="Channel"/> to remove.</param>
        public static void RemoveChannel(Channel channel) {
            foreach (var activeMember in channel.ActiveMembers) {
                MoveToChannel(activeMember, GetMainChannel());
            }

            Pool.Server.Channels.Remove(channel);
            Pool.Server.DataProvider.Save();
        }
    }

    // TODO: Add docs
    [JsonConverter(typeof(StringEnumConverter))]
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
