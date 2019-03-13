using System;
using Neo.Core.Config;
using Neo.Core.Management;
using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.Message"/> package.
    /// </summary>
    public class MessagePackageContent
    {
        /// <summary>
        ///     The internal id of the sender of this message.
        /// </summary>
        public Guid SenderId { get; set; }

        /// <summary>
        ///     The identity of the sender of this message.
        /// </summary>
        public Identity Identity { get; set; }

        /// <summary>
        ///     The content of this message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     The time this message was composed.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        ///     The type of this message.
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        ///     The internal id of the <see cref="Channel"/> this message belongs to.
        /// </summary>
        public Guid ChannelId { get; set; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="MessagePackageContent"/> class.
        /// </summary>
        /// <param name="senderId">The internal id of the sender of this message.</param>
        /// <param name="identity">The identity of the sender of this message.</param>
        /// <param name="message">The content of this message.</param>
        /// <param name="timestamp">The time this message was composed.</param>
        /// <param name="messageType">The type of this message.</param>
        /// <param name="channelId">The internal id of the channel this message belongs to.</param>
        public MessagePackageContent(Guid senderId, Identity identity, string message, DateTime timestamp, string messageType, Guid channelId) {
            this.SenderId = senderId;
            this.Identity = identity;
            this.Message = message;
            this.Timestamp = timestamp;
            this.MessageType = messageType;
            this.ChannelId = channelId;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessagePackageContent"/> class with the type set to <c>received</c>.
        /// </summary>
        /// <param name="senderId">The internal id of the sender of this message.</param>
        /// <param name="identity">The identity of the sender of this message.</param>
        /// <param name="message">The content of this message.</param>
        /// <param name="channelId">The internal id of the channel this message belongs to.</param>
        /// <returns>Returns the created content.</returns>
        public static MessagePackageContent GetReceivedMessage(Guid senderId, Identity identity, string message, Guid channelId) {
            return new MessagePackageContent(senderId, identity, message, DateTime.Now, "received", channelId);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessagePackageContent"/> class with the type set to <c>sent</c>.
        /// </summary>
        /// <param name="senderId">The internal id of the sender of this message.</param>
        /// <param name="identity">The identity of the sender of this message.</param>
        /// <param name="message">The content of this message.</param>
        /// <param name="channelId">The internal id of the channel this message belongs to.</param>
        /// <returns>Returns the created content.</returns>
        public static MessagePackageContent GetSentMessage(Guid senderId, Identity identity, string message, Guid channelId) {
            return new MessagePackageContent(senderId, identity, message, DateTime.Now, "sent", channelId);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MessagePackageContent"/> class with the type set to <c>system</c>.
        /// </summary>
        /// <param name="message">The content of this message.</param>
        /// <param name="channelId">The internal id of the channel this message belongs to.</param>
        /// <returns>Returns the created content.</returns>
        public static MessagePackageContent GetSystemMessage(string message, Guid channelId) {
            return new MessagePackageContent(Guid.Empty, UserManager.GetRoot().Identity, message, DateTime.Now, "system", channelId);
        }
    }
}
