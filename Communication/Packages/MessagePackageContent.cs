using System;
using Neo.Core.Config;
using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    public class MessagePackageContent
    {
        public Guid SenderId { get; set; }
        public Identity Identity { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; }
        public Guid ChannelId { get; set; }

        public MessagePackageContent(Guid senderId, Identity identity, string message, DateTime timestamp, string messageType, Guid channelId) {
            this.SenderId = senderId;
            this.Identity = identity;
            this.Message = message;
            this.Timestamp = timestamp;
            this.MessageType = messageType;
            this.ChannelId = channelId;
        }

        public static MessagePackageContent GetReceivedMessage(Guid senderId, Identity identity, string message, Guid channelId) {
            return new MessagePackageContent(senderId, identity, message, DateTime.Now, "received", channelId);
        }

        public static MessagePackageContent GetSentMessage(Guid senderId, Identity identity, string message, Guid channelId) {
            return new MessagePackageContent(senderId, identity, message, DateTime.Now, "sent", channelId);
        }

        public static MessagePackageContent GetSystemMessage(string message, Guid channelId) {
            return new MessagePackageContent(Guid.Empty, ConfigManager.Instance.Values.RootIdentity, message, DateTime.Now, "system", channelId);
        }
    }
}
