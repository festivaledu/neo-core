using System.Collections.Generic;
using Neo.Core.Communication;
using Neo.Core.Networking;
using Neo.Core.Shared;
using Newtonsoft.Json;

namespace Neo.Core.Management
{
    public static class UserManager
    {
        public static Account GetRoot() {
            return Pool.Server.Accounts.Find(g => g.Attributes.ContainsKey("neo.usertype") && g.Attributes["neo.usertype"].ToString() == "root");
        }

        public static void RefreshAccounts() {
            var accounts = JsonConvert.DeserializeObject<List<Account>>(JsonConvert.SerializeObject(Pool.Server.Accounts));
            accounts.ForEach(a => a.Password = null);

            Pool.Server.SendPackageTo(Target.All, new Package(PackageType.AccountListUpdate, accounts));
        }

        public static void RefreshUsers() {
            Pool.Server.SendPackageTo(Target.All, new Package(PackageType.UserListUpdate, Pool.Server.Users));
        }
    }
}
