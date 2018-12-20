using System;
using System.Collections.Generic;
using Neo.Core.Config;
using Neo.Core.Shared;
using WebSocketSharp.Server;

namespace Neo.Core.Networking
{
    /// <summary>
    ///     Represents a server used to handle all WebSocket communication.
    /// </summary>
    public abstract class BaseServer
    {
        private List<Client> Clients { get; set; } = new List<Client>();
        internal WebSocketSessionManager SessionManager { get; set; }
        private WebSocketServer WebSocketServer { get; set; }

        /// <summary>
        ///     Allows this instance to be accessed from the <see cref="Pool"/>.
        /// </summary>
        public void Register() {
            Pool.Server = this;
        }

        /// <summary>
        ///     Applies all necessary settings and starts the underlying <see cref="WebSocketSharp.Server.WebSocketServer"/>.
        /// </summary>
        public void Start() {
            WebSocketServer = new WebSocketServer($"ws://{ConfigManager.Instance["server.address", "0.0.0.0"]}:{ConfigManager.Instance["server.port", "42042"]}");
            WebSocketServer.AddWebSocketService<NeoWebSocketBehaviour>("/neo");
            WebSocketServer.Start();
        }

        /// <summary>
        ///     Stops the underlying <see cref="WebSocketSharp.Server.WebSocketServer"/>.
        /// </summary>
        public void Stop() {
            WebSocketServer.Stop();
        }

        public abstract void OnConnect(Client client);
        public abstract void OnDisconnect(string clientId, ushort code, string reason, bool wasClean);
        public abstract void OnError(string clientId, Exception ex, string message);
        public abstract void OnMessage(string clientId, string message);
    }
}
