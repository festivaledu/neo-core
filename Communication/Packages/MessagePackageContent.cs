using System;
using Neo.Core.Config;
using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    public class MessagePackageContent
    {
        public Identity Identity { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageType { get; set; }
        public Guid ChannelId { get; set; }

        public MessagePackageContent(Identity identity, string message, DateTime timestamp, string messageType, Guid channelId) {
            this.Identity = identity;
            this.Message = message;
            this.Timestamp = timestamp;
            this.MessageType = messageType;
            this.ChannelId = channelId;
        }

        public static MessagePackageContent GetReceivedMessage(Identity identity, string message, Guid channelId) {
            return new MessagePackageContent(identity, message, DateTime.Now, "received", channelId);
        }

        public static MessagePackageContent GetSentMessage(Identity identity, string message, Guid channelId) {
            return new MessagePackageContent(identity, message, DateTime.Now, "sent", channelId);
        }

        public static MessagePackageContent GetSystemMessage(string message, Guid channelId) {
            return new MessagePackageContent(ConfigManager.Instance.Values.RootIdentity, message, DateTime.Now, "system", channelId);
        }
    }
}
