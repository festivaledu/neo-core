using WebSocketSharp;
using WebSocketSharp.Server;

namespace Neo.Core.Networking
{
    /// <summary>
    ///     Represents a client connected to a <see cref="BaseServer"/>.
    /// </summary>
    public class Client
    {
        /// <summary>
        ///     Id used by the <see cref="WebSocketSessionManager"/> to identify the <see cref="IWebSocketSession"/> this <see cref="Socket"/> belongs to.
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        ///     Determines whether this <see cref="Client"/> is an official client.
        /// </summary>
        public bool IsOfficial { get; set; }

        /// <summary>
        ///     <see cref="WebSocket"/> used for sending and receiving data.
        /// </summary>
        public WebSocket Socket { get; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="clientId">Id used by the <see cref="WebSocketSessionManager"/> to identify the <see cref="IWebSocketSession"/> this <see cref="Socket"/> belongs to.</param>
        /// <param name="socket"><see cref="WebSocket"/> used for sending and receiving data.</param>
        internal Client(string clientId, WebSocket socket) {
            this.ClientId = clientId;
            this.Socket = socket;

            var wssv = new WebSocketServer("ws://localhost:42421");
            wssv.AddWebSocketService<NeoWebSocketBehaviour>("/neo");
        }
    }
}
