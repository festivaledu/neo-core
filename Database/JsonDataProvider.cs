using System.Collections.Generic;
using System.IO;
using Neo.Core.Networking;
using Neo.Core.Shared;
using Newtonsoft.Json;

namespace Neo.Core.Database
{
    public sealed class JsonDataProvider : DataProvider
    {
        private readonly string directoryPath;

        public JsonDataProvider(BaseServer server, string directoryPath) : base(server) {
            this.directoryPath = directoryPath;
        }

        public override void Load() {
            Logger.Instance.Log(LogLevel.Info, $"Loading data from {directoryPath} using a JsonDataProvider");

            var dataPath = new DirectoryInfo(directoryPath);

            var accountsPath = new FileInfo(Path.Combine(dataPath.FullName, "accounts.json"));
            var channelsPath = new FileInfo(Path.Combine(dataPath.FullName, "channels.json"));
            var groupsPath = new FileInfo(Path.Combine(dataPath.FullName, "groups.json"));

            if (!dataPath.Exists) {
                Logger.Instance.Log(LogLevel.Warn, "Specified data directory doesn't exist. Creating...");

                dataPath.Create();

                File.WriteAllText(accountsPath.FullName, JsonConvert.SerializeObject(server.Accounts, Formatting.Indented));
                File.WriteAllText(channelsPath.FullName, JsonConvert.SerializeObject(server.Channels, Formatting.Indented));
                File.WriteAllText(groupsPath.FullName, JsonConvert.SerializeObject(server.Groups, Formatting.Indented));

                Logger.Instance.Log(LogLevel.Ok, "Data directory successfully created");
            }

            if (!accountsPath.Exists || accountsPath.Length == 0) {
                File.WriteAllText(accountsPath.FullName, JsonConvert.SerializeObject(server.Accounts, Formatting.Indented));
                Logger.Instance.Log(LogLevel.Warn, "Data file \"accounts.json\" didn't existed and was successfully created.");
            }

            if (!channelsPath.Exists || channelsPath.Length == 0) {
                File.WriteAllText(channelsPath.FullName, JsonConvert.SerializeObject(server.Channels, Formatting.Indented));
                Logger.Instance.Log(LogLevel.Warn, "Data file \"channels.json\" didn't existed and was successfully created.");
            }

            if (!groupsPath.Exists || groupsPath.Length == 0) {
                File.WriteAllText(groupsPath.FullName, JsonConvert.SerializeObject(server.Groups, Formatting.Indented));
                Logger.Instance.Log(LogLevel.Warn, "Data file \"groups.json\" didn't existed and was successfully created.");
            }
            
            server.Accounts = JsonConvert.DeserializeObject<List<Account>>(File.ReadAllText(accountsPath.FullName));
            server.Channels = JsonConvert.DeserializeObject<List<Channel>>(File.ReadAllText(channelsPath.FullName));
            server.Groups = JsonConvert.DeserializeObject<List<Group>>(File.ReadAllText(groupsPath.FullName));

            Logger.Instance.Log(LogLevel.Ok, "Data successfully loaded");
        }

        public override void Save() {
            Logger.Instance.Log(LogLevel.Info, $"Saving data to {directoryPath} using a JsonDataProvider");
            var dataPath = new DirectoryInfo(directoryPath);

            var accountsPath = new FileInfo(Path.Combine(dataPath.FullName, "accounts.json"));
            var channelsPath = new FileInfo(Path.Combine(dataPath.FullName, "channels.json"));
            var groupsPath = new FileInfo(Path.Combine(dataPath.FullName, "groups.json"));
            
            File.WriteAllText(accountsPath.FullName, JsonConvert.SerializeObject(server.Accounts, Formatting.Indented));
            File.WriteAllText(channelsPath.FullName, JsonConvert.SerializeObject(server.Channels, Formatting.Indented));
            File.WriteAllText(groupsPath.FullName, JsonConvert.SerializeObject(server.Groups, Formatting.Indented));

            Logger.Instance.Log(LogLevel.Ok, "Data successfully saved");
        }
    }
}
