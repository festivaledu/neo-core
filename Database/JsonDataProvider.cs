using System.Collections.Generic;
using System.IO;
using Neo.Core.Shared;
using Newtonsoft.Json;

namespace Neo.Core.Database
{
    public sealed class JsonDataProvider : DataProvider
    {
        private readonly string directoryPath;

        public JsonDataProvider(string directoryPath) {
            this.directoryPath = directoryPath;
        }

        public override void Load() {
            var dataPath = new DirectoryInfo(directoryPath);

            var accountsPath = new FileInfo(Path.Combine(dataPath.FullName, "accounts.json"));
            var channelsPath = new FileInfo(Path.Combine(dataPath.FullName, "channels.json"));
            var groupsPath = new FileInfo(Path.Combine(dataPath.FullName, "groups.json"));

            if (!dataPath.Exists) {
                dataPath.Create();

                File.WriteAllText(accountsPath.FullName, JsonConvert.SerializeObject(Pool.Server.Accounts, Formatting.Indented));
                File.WriteAllText(channelsPath.FullName, JsonConvert.SerializeObject(Pool.Server.Channels, Formatting.Indented));
                File.WriteAllText(groupsPath.FullName, JsonConvert.SerializeObject(Pool.Server.Groups, Formatting.Indented));
            }

            if (!accountsPath.Exists || accountsPath.Length == 0) {
                throw new FileNotFoundException("The specified folder does not contain a valid account data file.");
            }

            if (!channelsPath.Exists || channelsPath.Length == 0) {
                throw new FileNotFoundException("The specified folder does not contain a valid channel data file.");
            }

            if (!groupsPath.Exists || groupsPath.Length == 0) {
                throw new FileNotFoundException("The specified folder does not contain a valid group data file.");
            }

            try {
                Pool.Server.Accounts = JsonConvert.DeserializeObject<List<Account>>(File.ReadAllText(accountsPath.FullName));
            } catch {
                throw new FileNotFoundException("The specified folder does not contain a valid account data file.");
            }

            try {
                Pool.Server.Channels = JsonConvert.DeserializeObject<List<Channel>>(File.ReadAllText(channelsPath.FullName));
            } catch {
                throw new FileNotFoundException("The specified folder does not contain a valid channel data file.");
            }

            try {
                Pool.Server.Groups = JsonConvert.DeserializeObject<List<Group>>(File.ReadAllText(groupsPath.FullName));
            } catch {
                throw new FileNotFoundException("The specified folder does not contain a valid group data file.");
            }
        }

        public override void Save() {
            var dataPath = new DirectoryInfo(directoryPath);

            var accountsPath = new FileInfo(Path.Combine(dataPath.FullName, "accounts.json"));
            var channelsPath = new FileInfo(Path.Combine(dataPath.FullName, "channels.json"));
            var groupsPath = new FileInfo(Path.Combine(dataPath.FullName, "groups.json"));
            
            File.WriteAllText(accountsPath.FullName, JsonConvert.SerializeObject(Pool.Server.Accounts, Formatting.Indented));
            File.WriteAllText(channelsPath.FullName, JsonConvert.SerializeObject(Pool.Server.Channels, Formatting.Indented));
            File.WriteAllText(groupsPath.FullName, JsonConvert.SerializeObject(Pool.Server.Groups, Formatting.Indented));
        }
    }
}
