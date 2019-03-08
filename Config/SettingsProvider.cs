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
    public static class SettingsProvider
    {
        public static bool EditSettings(string scope, dynamic model) {
            if (scope == "server") {
                ConfigManager.Instance.Values = JsonConvert.DeserializeObject<ConfigValues>(JsonConvert.SerializeObject(model));
                ConfigManager.Instance.Save();

                Target.All.SendPackageTo(new Package(PackageType.MetaResponse, new ServerMetaPackageContent {
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
            }
            
            return true;
        }

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
