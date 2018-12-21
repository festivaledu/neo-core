using System;
using System.Collections.Generic;
using System.Reflection;
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
            Logger.Instance.Log(LogLevel.Info, $"Generating RSA key pair with a key size of {ConfigManager.Instance.Values.RSAKeySize} bytes...");
            using (var rsa = new RSACryptoServiceProvider(ConfigManager.Instance.Values.RSAKeySize)) {
                RSAPublicParameters = rsa.ExportParameters(false);
                RSAPrivateParameters = rsa.ExportParameters(true);
            }
            Logger.Instance.Log(LogLevel.Ok, "RSA key pair successfully generated.");

            Logger.Instance.Log(LogLevel.Info, $"Attempting to start WebSocket server on ws://{ConfigManager.Instance.Values.ServerAddress}:{ConfigManager.Instance.Values.ServerPort}...");
            webSocketServer = new WebSocketServer($"ws://{ConfigManager.Instance.Values.ServerAddress}:{ConfigManager.Instance.Values.ServerPort}");
            webSocketServer.AddWebSocketService<NeoWebSocketBehaviour>("/neo");
            webSocketServer.Start();
            Logger.Instance.Log(LogLevel.Ok, "WebSocket server successfully started.");
        }

        /// <summary>
        ///     Stops the underlying <see cref="WebSocketServer"/>.
        /// </summary>
        public void Stop() {
            Logger.Instance.Log(LogLevel.Info, $"Attempting to stop WebSocket server...");
            webSocketServer.Stop();
            Logger.Instance.Log(LogLevel.Ok, "WebSocket server successfully stopped.");
        }

        public abstract void OnConnect(Client client);
        public abstract void OnDisconnect(string clientId, ushort code, string reason, bool wasClean);
        public abstract void OnError(string clientId, Exception ex, string message);
        public abstract void OnPackage(string clientId, Package package);
    }
}
