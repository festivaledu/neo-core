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

        public MessagePackageContent(Identity identity, string message, DateTime timestamp, string messageType) {
            Identity = identity;
            Message = message;
            Timestamp = timestamp;
            MessageType = messageType;
        }

        public static MessagePackageContent GetReceivedMessage(Identity identity, string message) {
            return new MessagePackageContent(identity, message, DateTime.Now, "received");
        }

        public static MessagePackageContent GetSentMessage(Identity identity, string message) {
            return new MessagePackageContent(identity, message, DateTime.Now, "sent");
        }

        public static MessagePackageContent GetSystemMessage(string message) {
            return new MessagePackageContent(ConfigManager.Instance.Values.RootIdentity, message, DateTime.Now, "system");
        }
    }
}
