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
using Neo.Core.Management;
using Neo.Core.Shared;
using WebSocketSharp.Server;

namespace Neo.Core.Networking
{
    /// <summary>
    ///     Represents a server used to handle all WebSocket communication.
    /// </summary>
    public abstract class BaseServer
    {
        /// <summary>
        ///     Contains all known <see cref="Account"/>s.
        /// </summary>
        public List<Account> Accounts { get; set; } = new List<Account>();

        /// <summary>
        ///     Contains all known <see cref="Channel"/>s.
        /// </summary>
        public List<Channel> Channels { get; set; } = new List<Channel>();

        /// <summary>
        ///     Contains all connected <see cref="Client"/>s.
        /// </summary>
        public List<Client> Clients { get; set; } = new List<Client>();

        /// <summary>
        ///     Contains all known <see cref="Group"/>s.
        /// </summary>
        public List<Group> Groups { get; set; } = new List<Group>();

        /// <summary>
        ///     Contains all connected <see cref="User"/>s.
        /// </summary>
        public List<User> Users { get; set; } = new List<User>();

        public Dictionary<string, string> KnownPermissions { get; set; } = new Dictionary<string, string> {
            { "neo.channel.create", "Channel erstellen" },
            { "neo.channel.delete", "Channel löschen" },
            { "neo.channel.join.$", "Channel betreten" },
            { "neo.channel.join.ignoreblacklist", "Channel trotz Blacklist betreten" },
            { "neo.channel.join.ignorelimit", "Channel trotz Limit betreten" },
            { "neo.channel.join.ignorepassword", "Channel trotz Passwort betreten" },
            { "neo.channel.join.ignorewhitelist", "Channel trotz Whitelist betreten" },
            { "neo.global.read", "Nachrichten lesen" },
            { "neo.global.write", "Nachrichten schreiben" },
            { "neo.group.create", "Gruppe erstellen" },
            { "neo.group.delete", "Gruppe löschen" },
            { "neo.moderate.ban", "Benutzer bannen" },
            { "neo.moderate.kick", "Benutzer kicken" }
        };

        public DataProvider DataProvider { get; set; }
        
        // ReSharper disable once InconsistentNaming
        public RSAParameters RSAPublicParameters { get; private set; }

        // ReSharper disable once InconsistentNaming
        internal RSAParameters RSAPrivateParameters { get; private set; }
        internal WebSocketSessionManager SessionManager { get; set; }

        protected string dataPath;

        private WebSocketServer webSocketServer;

        public abstract Task OnConnect(Client client, WebSocketSessionManager sessions);
        public abstract Task OnDisconnect(string clientId, ushort code, string reason, bool wasClean);
        public abstract Task OnError(string clientId, Exception ex, string message);
        public abstract Task OnPackage(string clientId, Package package);

        /// <summary>
        ///     Returns the associated <see cref="User"/> to a <see cref="Client"/>.
        /// </summary>
        /// <param name="client">The <see cref="Client"/> to get the <see cref="User"/> from.</param>
        /// <returns>Returns the <see cref="User"/>.</returns>
        protected User GetUser(Client client) {
            return GetUser(client.ClientId);
        }

        /// <summary>
        ///     Returns the associated <see cref="User"/> to a <see cref="Client"/>.
        /// </summary>
        /// <param name="clientId">The internal id of the <see cref="Client"/> to get the <see cref="User"/> from.</param>
        /// <returns>Returns the <see cref="User"/>.</returns>
        protected User GetUser(string clientId) {
            return Users.Find(u => u.Client.ClientId == clientId);
        }

        /// <summary>
        ///     Initializes this <see cref="BaseServer"/>.
        /// </summary>
        /// <param name="configPath">The path to the config file.</param>
        /// <param name="dataDirectoryPath">The path to the data directory.</param>
        /// <param name="pluginDirectoryPath">The path to the plugins directory.</param>
        public void Initialize(string configPath, string dataDirectoryPath, string pluginDirectoryPath) {
            ConfigManager.Instance.Load(configPath);
            Pool.Server = this;
            this.dataPath = dataDirectoryPath;
            DataProvider = new JsonDataProvider(this, dataDirectoryPath);
            DataProvider.Load();

            // Create root account
            if (UserManager.GetRoot() == null) {
                Accounts.Add(new Account {
                    Attributes = new Dictionary<string, object> {
                        { "neo.usertype", "root" }
                    },
                    Email = "root@internal.neo",
                    Identity = ConfigManager.Instance.Values.RootIdentity,
                    Password = ConfigManager.Instance.Values.RootPassword,
                    Permissions = new Dictionary<string, Permission> {
                        { "*", Permission.Allow }
                    }
                });
                Logger.Instance.Log(LogLevel.Debug, "No root account existed. Default root account created");
            }

            // Create main channel
            if (ChannelManager.GetMainChannel() == null) {
                Channels.Insert(0, new Channel {
                    Attributes = new Dictionary<string, object> {
                        { "neo.channeltype", "main" }
                    },
                    EndOfLifetime = DateTime.MaxValue,
                    Id = "main",
                    Lifetime = Lifespan.Permanent,
                    Limit = -1,
                    Name = "Main",
                    Owner = UserManager.GetRoot().InternalId
                });

                ChannelManager.GetMainChannel().MemberIds.AddRange(Accounts.FindAll(a => a.Email != "root@internal.neo").Select(a => a.InternalId));
                Logger.Instance.Log(LogLevel.Debug, "No main channel existed. Default main channel created");
            }

            if (GroupManager.GetAdminGroup() == null) {
                Groups.Add(new Group {
                    Attributes = new Dictionary<string, object> {
                        { "neo.grouptype", "admin" }
                    },
                    Id = "admins",
                    Name = "Administratoren",
                    Permissions = new Dictionary<string, Permission> {
                        // TODO: Fix default admin group rights
                        { "neo.*", Permission.Allow }
                    },
                    SortValue = 999
                });
                DataProvider.Save();
                Logger.Instance.Log(LogLevel.Debug, "No admin group existed. Default admin group created");
            }

            if (GroupManager.GetGuestGroup() == null) {
                Groups.Add(new Group {
                    Attributes = new Dictionary<string, object> {
                        { "neo.grouptype", "user" }
                    },
                    Id = "users",
                    Name = "Benutzer",
                    Permissions = new Dictionary<string, Permission> {
                        // TODO: Fix default user group rights
                        { "neo.*", Permission.Allow }
                    },
                    SortValue = 1
                });
                DataProvider.Save();
                Logger.Instance.Log(LogLevel.Debug, "No user group existed. Default user group created");
            }

            if (GroupManager.GetGuestGroup() == null) {
                Groups.Add(new Group {
                    Attributes = new Dictionary<string, object> {
                        { "neo.grouptype", "guest" }
                    },
                    Id = "guests",
                    Name = "Gäste",
                    Permissions = new Dictionary<string, Permission> {
                        // TODO: Fix default guest group rights
                        { "neo.*", Permission.Allow }
                    },
                    SortValue = 0
                });
                DataProvider.Save();
                Logger.Instance.Log(LogLevel.Debug, "No guest group existed. Default guest group created");
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
        /// <param name="package">The <see cref="Package" /> to send.</param>
        public void SendPackageTo(Target target, Package package) {
            foreach (var client in Clients.FindAll(c => target.Targets.Contains(c.ClientId))) {
                client.SendPackage(package);
            }
        }

        /// <summary>
        ///     Sends a <see cref="Package" /> to a client.
        /// </summary>
        /// <param name="client">The client of the recipient of the <see cref="Package"/>.</param>
        /// <param name="package">The <see cref="Package" /> to send.</param>
        public void SendPackageTo(Client client, Package package) {
            SendPackageTo(client.ClientId, package);
        }

        /// <summary>
        ///     Sends a <see cref="Package" /> to a client.
        /// </summary>
        /// <param name="clientId">The client id of the recipient of the <see cref="Package"/>.</param>
        /// <param name="package">The <see cref="Package" /> to send.</param>
        public void SendPackageTo(string clientId, Package package) {
            Clients.Find(_ => _.ClientId == clientId)?.SendPackage(package);
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
            PluginLoader.Plugins.ForEach(_ => _.OnDispose());

            ConfigManager.Instance.Save();
            DataProvider.Save();

            Logger.Instance.Log(LogLevel.Info, "Attempting to stop WebSocket server...");

            webSocketServer.Stop();

            Logger.Instance.Log(LogLevel.Ok, "WebSocket server successfully stopped");
        }
    }
}