using WebSocketSharp;

namespace Neo.Core.Networking
{
    internal class Client
    {
        internal string ClientId { get; set; }
        internal WebSocket Socket { get; set; }

        internal Client(string clientId, WebSocket socket) {
            this.ClientId = clientId;
            this.Socket = socket;
        }
    }
}
