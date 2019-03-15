using System.Collections.Generic;
using Neo.Core.Communication;
using Neo.Core.Communication.Packages;
using Neo.Core.Networking;
using Neo.Core.Shared;
using Newtonsoft.Json;

namespace Neo.Core.Management
{
    /// <summary>
    ///     Provides methods to manage <see cref="User"/>s.
    /// </summary>
    public static class UserManager
    {
        /// <summary>
        ///     Returns the root <see cref="Account"/>.
        /// </summary>
        /// <returns>Returns the root <see cref="Account"/>.</returns>
        public static Account GetRoot() {
            return Pool.Server.Accounts.Find(_ => _.Attributes.ContainsKey("neo.usertype") && _.Attributes["neo.usertype"].ToString() == "root");
        }

        /// <summary>
        ///     Updates the list of <see cref="Account"/>s for each connected <see cref="User"/>.
        /// </summary>
        public static void RefreshAccounts() {
            var accounts = JsonConvert.DeserializeObject<List<Account>>(JsonConvert.SerializeObject(Pool.Server.Accounts));
            accounts.ForEach(_ => {
                _.Password = null;
                _.Identity.AvatarFileExtension = Pool.Server.AvatarServerAvailable ? _.Identity.AvatarFileExtension : "";
            });

            Target.All.SendPackage(new Package(PackageType.AccountListUpdate, accounts));
        }

        /// <summary>
        ///     Updates the list of <see cref="User"/>s for each connected <see cref="User"/>.
        /// </summary>
        public static void RefreshUsers() {
            Target.All.SendPackage(new Package(PackageType.UserListUpdate, Pool.Server.Users));
        }
    }
}
