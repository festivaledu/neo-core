using WebSocketSharp;
using WebSocketSharp.Server;

namespace Neo.Core.Networking
{
    public class Client
    {
        private string ClientId { get; set; }
        private WebSocket Socket { get; set; }

        internal Client(string clientId, WebSocket socket) {
            this.ClientId = clientId;
            this.Socket = socket;

            var wssv = new WebSocketServer("ws://localhost:42421");
            wssv.AddWebSocketService<NeoWebSocketBehaviour>("/neo");
        }
    }
}
