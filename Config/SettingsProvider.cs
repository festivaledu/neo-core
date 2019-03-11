using System.Linq;
using System.Reflection;
using Neo.Core.Communication;
using Neo.Core.Communication.Packages;
using Neo.Core.Management;
using Neo.Core.Networking;
using Neo.Core.Shared;
using Newtonsoft.Json;

namespace Neo.Core.Config
{
    /// <summary>
    ///     Provides methods for editing settings.
    /// </summary>
    public static class SettingsProvider
    {
        /// <summary>
        ///     Applies the given model on a scope.
        /// </summary>
        /// <param name="scope">The scope to apply the model on.</param>
        /// <param name="model">The model to apply.</param>
        /// <returns>Returns <c>true</c> when the model was applied successfully, otherwise <c>false</c>.</returns>
        public static bool EditSettings(string scope, dynamic model) {
            if (scope == "server") {
                ConfigManager.Instance.Values = JsonConvert.DeserializeObject<ConfigValues>(JsonConvert.SerializeObject(model));
                ConfigManager.Instance.Save();

                Target.All.SendPackage(new Package(PackageType.MetaResponse, new ServerMetaResponsePackageContent {
                    GuestsAllowed = ConfigManager.Instance.Values.GuestsAllowed,
                    Name = ConfigManager.Instance.Values.ServerName,
                    RegistrationAllowed = ConfigManager.Instance.Values.RegistrationAllowed
                }));
            } else if (scope == "account") {
                Account account = JsonConvert.DeserializeObject<Account>(JsonConvert.SerializeObject(model));
                var index = Pool.Server.Accounts.FindIndex(a => a.InternalId.Equals(account.InternalId));

                if (index == -1) {
                    return false;
                }

                Pool.Server.Accounts[index] = account;
                Pool.Server.DataProvider.Save();
            } else if (scope == "group") {
                Group group = JsonConvert.DeserializeObject<Group>(JsonConvert.SerializeObject(model));
                var index = Pool.Server.Groups.FindIndex(g => g.InternalId.Equals(group.InternalId));

                if (index == -1) {
                    return false;
                }

                Pool.Server.Groups[index] = group;
                Pool.Server.DataProvider.Save();
                GroupManager.RefreshGroups();
            } else if (scope == "channel") {
                Channel channel = JsonConvert.DeserializeObject<Channel>(JsonConvert.SerializeObject(model));
                var index = Pool.Server.Channels.FindIndex(c => c.InternalId.Equals(channel.InternalId));

                if (index == -1) {
                    return false;
                }

                if (channel.Password == "true") {
                    channel.Password = Pool.Server.Channels[index].Password;
                }

                Pool.Server.Channels[index] = channel;
                Pool.Server.DataProvider.Save();
                ChannelManager.RefreshChannels();
            }
            
            return true;
        }

        /// <summary>
        ///     Creates a <see cref="OpenSettingsResponsePackageContent"/> with the model for a given scope to edit.
        /// </summary>
        /// <param name="scope">The scope to create a model for.</param>
        /// <returns>Returns the created content.</returns>
        public static OpenSettingsResponsePackageContent OpenSettings(string scope) {
            OpenSettingsResponsePackageContent response = null;

            if (scope == "server") {
                var model = ConfigManager.Instance.Values;
                var modelType = typeof(ConfigValues);

                response = new OpenSettingsResponsePackageContent(model);

                var properties = modelType.GetProperties().ToList().FindAll(pi => pi.GetCustomAttribute<EditablePropertyAttribute>() != null);
                foreach (var property in properties) {
                    response.Titles.Add(property.Name.ToLower(), property.GetCustomAttribute<EditablePropertyAttribute>().Title);
                }
            }

            return response;
        }
    }
}
