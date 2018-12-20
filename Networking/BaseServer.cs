using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Neo.Core.Communication;
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
        public List<Account> Accounts { get; set; } = new List<Account>();
        public List<Channel> Channels { get; set; } = new List<Channel>();
        public List<Group> Groups { get; set; } = new List<Group>();
        public List<User> Users { get; set; } = new List<User>();

        internal List<Client> Clients { get; set; } = new List<Client>();
        // ReSharper disable once InconsistentNaming
        internal RSAParameters RSAPublicParameters { get; private set; }
        // ReSharper disable once InconsistentNaming
        internal RSAParameters RSAPrivateParameters { get; private set; }
        internal WebSocketSessionManager SessionManager { get; set; }

        private WebSocketServer webSocketServer;

        /// <summary>
        ///     Allows this instance to be accessed from the <see cref="Pool"/>.
        /// </summary>
        public void Register() {
            Pool.Server = this;
        }

        /// <summary>
        ///     Applies all necessary settings and starts the underlying <see cref="WebSocketServer"/>.
        /// </summary>
        public void Start() {
            using (var rsa = new RSACryptoServiceProvider(ConfigManager.Instance["rsa.keysize", 4096])) {
                RSAPublicParameters = rsa.ExportParameters(false);
                RSAPrivateParameters = rsa.ExportParameters(true);
            }

            webSocketServer = new WebSocketServer($"ws://{ConfigManager.Instance["server.address", "0.0.0.0"]}:{ConfigManager.Instance["server.port", "42042"]}");
            webSocketServer.AddWebSocketService<NeoWebSocketBehaviour>("/neo");
            webSocketServer.Start();
        }

        /// <summary>
        ///     Stops the underlying <see cref="WebSocketServer"/>.
        /// </summary>
        public void Stop() {
            webSocketServer.Stop();
        }

        public abstract void OnConnect(Client client);
        public abstract void OnDisconnect(string clientId, ushort code, string reason, bool wasClean);
        public abstract void OnError(string clientId, Exception ex, string message);
        public abstract void OnPackage(string clientId, Package package);
    }
}
