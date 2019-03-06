using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Neo.Core.Authorization;
using Neo.Core.Communication;
using Neo.Core.Config;
using Neo.Core.Database;
using Neo.Core.Extensibility;
using Neo.Core.Extensibility.Events;
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

        public List<Client> Clients { get; set; } = new List<Client>();
        // ReSharper disable once InconsistentNaming
        public RSAParameters RSAPublicParameters { get; private set; }
        // ReSharper disable once InconsistentNaming
        internal RSAParameters RSAPrivateParameters { get; private set; }
        internal WebSocketSessionManager SessionManager { get; set; }

        protected DataProvider dataProvider;
        private WebSocketServer webSocketServer;

        public abstract Task OnConnect(Client client);
        public abstract Task OnDisconnect(string clientId, ushort code, string reason, bool wasClean);
        public abstract Task OnError(string clientId, Exception ex, string message);
        public abstract Task OnPackage(string clientId, Package package);

        protected User GetUser(string clientId) {
            return Users.Find(u => u.Client.ClientId == clientId);
        }

        public void Initialize(string configPath, string dataDirectoryPath, string pluginDirectoryPath) {
            ConfigManager.Instance.Load(configPath);
            Pool.Server = this;
            dataProvider = new JsonDataProvider(this, dataDirectoryPath);
            dataProvider.Load();

            // Create root account
            Accounts.Insert(0, new Account {
                Attributes = new Dictionary<string, object> {
                    { "instance.neo.origin", "neo.server" }
                },
                Email = "root@internal.neo",
                Identity = ConfigManager.Instance.Values.RootIdentity,
                Password = ConfigManager.Instance.Values.RootPassword,
                Permissions = new Dictionary<string, Permission> {
                    { "*", Permission.Allow }
                }
            });
            Logger.Instance.Log(LogLevel.Debug, "Root account created");

            // Create main channel
            Channels.Insert(0, new Channel {
                Attributes = new Dictionary<string, object> {
                    { "instance.neo.origin", "neo.server" }
                },
                EndOfLifetime = DateTime.MaxValue,
                Id = "main",
                Lifetime = Lifespan.Permanent,
                Limit = int.MaxValue,
                Name = "Main",
                Owner = Accounts[0].InternalId
            });

            Channels[0].MemberIds.AddRange(Accounts.FindAll(a => a.Email != "root@internal.neo").Select(a => a.InternalId));
            Logger.Instance.Log(LogLevel.Debug, "Main channel created");

            var existingGuestGroup = Groups.Find(g => g.Attributes.ContainsKey("neo.grouptype") && g.Attributes["neo.grouptype"].ToString() == "guest");
            if (existingGuestGroup != null) {
                Groups.Remove(existingGuestGroup);
                Groups.Insert(0, existingGuestGroup);
            } else {
                Groups.Insert(0, new Group {
                    Attributes = new Dictionary<string, object> {
                        { "neo.grouptype", "guest" }
                    },
                    Id = "guests",
                    Name = "Gäste",
                    Permissions = new Dictionary<string, Permission> {
                        // TODO: Fix default guest group rights
                        { "neo.*", Permission.Allow }
                    },
                    SortValue = 0,
                });
                dataProvider.Save();
                Logger.Instance.Log(LogLevel.Debug, "No guest group existed. Default guest group created");
            }

            var existingUserGroup = Groups.Find(g => g.Attributes.ContainsKey("neo.grouptype") && g.Attributes["neo.grouptype"].ToString() == "user");
            if (existingUserGroup != null) {
                Groups.Remove(existingUserGroup);
                Groups.Insert(1, existingUserGroup);
            } else {
                Groups.Insert(1, new Group {
                    Attributes = new Dictionary<string, object> {
                        { "neo.grouptype", "user" }
                    },
                    Id = "users",
                    Name = "Benutzer",
                    Permissions = new Dictionary<string, Permission> {
                        // TODO: Fix default user group rights
                        { "neo.*", Permission.Allow }
                    },
                    SortValue = 1,
                });
                dataProvider.Save();
                Logger.Instance.Log(LogLevel.Debug, "No user group existed. Default user group created");
            }

            foreach (var pluginFile in new DirectoryInfo(pluginDirectoryPath).EnumerateFiles("*.dll")) {
                PluginLoader.InitializePlugin(pluginFile.FullName);
            }

            EventService.RaiseEvent(EventType.ServerInitialized, this);
        }

        /// <summary>
        ///     Sends a <see cref="Package"/> to a <see cref="Target"/>.
        /// </summary>
        /// <param name="target">The recipients of the <see cref="Package"/>.</param>
        /// <param name="package">The <see cref="Package"/> to send.</param>
        public void SendPackageTo(Target target, Package package) {
            foreach (var client in Clients.FindAll(c => target.Targets.Contains(c.ClientId))) {
                // TODO
                client.SendPackage(package);
            }
        }

        /// <summary>
        ///     Sends a <see cref="Package"/> to a client.
        /// </summary>
        /// <param name="clientId">The recipients of the <see cref="Package"/>.</param>
        /// <param name="package">The <see cref="Package"/> to send.</param>
        public void SendPackageTo(string clientId, Package package) {
            Clients.Find(c => c.ClientId == clientId)?.SendPackage(package);
        }

        /// <summary>
        ///     Applies all necessary settings and starts the underlying <see cref="WebSocketServer"/>.
        /// </summary>
        public void Start() {
            //Logger.Instance.Log(LogLevel.Info, $"Generating RSA key pair with a key size of {ConfigManager.Instance.Values.RSAKeySize} bytes. This may take a while...");
            //using (var rsa = new RSACryptoServiceProvider(ConfigManager.Instance.Values.RSAKeySize)) {
            //    RSAPublicParameters = rsa.ExportParameters(false);
            //    RSAPrivateParameters = rsa.ExportParameters(true);
            //}
            //Logger.Instance.Log(LogLevel.Ok, "RSA key pair successfully generated");

            Logger.Instance.Log(LogLevel.Info, $"Attempting to start WebSocket server on ws://{ConfigManager.Instance.Values.ServerAddress}:{ConfigManager.Instance.Values.ServerPort}...");
            webSocketServer = new WebSocketServer($"ws://{ConfigManager.Instance.Values.ServerAddress}:{ConfigManager.Instance.Values.ServerPort}");
            webSocketServer.AddWebSocketService<NeoWebSocketBehaviour>("/neo");
            webSocketServer.Start();
            Logger.Instance.Log(LogLevel.Info, $"Use {Dns.GetHostEntry(Dns.GetHostName()).AddressList.ToList().Find(ip => ip.AddressFamily == AddressFamily.InterNetwork)} to connect to this instance.", true);
            Logger.Instance.Log(LogLevel.Ok, "WebSocket server successfully started. Waiting for connections...");
        }

        /// <summary>
        ///     Stops the underlying <see cref="WebSocketServer"/>.
        /// </summary>
        public void Stop() {
            ConfigManager.Instance.Save();
            dataProvider.Save();
            Logger.Instance.Log(LogLevel.Info, "Attempting to stop WebSocket server...");
            webSocketServer.Stop();
            Logger.Instance.Log(LogLevel.Ok, "WebSocket server successfully stopped");
        }
    }
}
