using System;
using System.Collections.Generic;
using Neo.Core.Shared;
using WebSocketSharp.Server;

namespace Neo.Core.Networking
{
    public abstract class BaseServer
    {
        private List<Client> Clients { get; set; }
        internal WebSocketSessionManager SessionManager { get; set; }
        private WebSocketServer WebSocketServer { get; set; }

        public void Register() {
            Pool.Server = this;
        }

        public void Start() {
            WebSocketServer = new WebSocketServer("ws://localhost:42421");
            WebSocketServer.AddWebSocketService<NeoWebSocketBehaviour>("/neo");
            WebSocketServer.Start();
        }

        public void Stop() {
            WebSocketServer.Stop();
        }

        public abstract void OnConnect(Client client);
        public abstract void OnDisconnect(string clientId, ushort code, string reason, bool wasClean);
        public abstract void OnError(string clientId, Exception ex, string message);
        public abstract void OnMessage(string clientId, string message);
    }
}
