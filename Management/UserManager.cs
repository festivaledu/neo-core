﻿using Neo.Core.Communication;
using Neo.Core.Networking;
using Neo.Core.Shared;

namespace Neo.Core.Management
{
    public static class UserManager
    {
        public static void RefreshUsers() {
            // TODO: Send user data to members
            Pool.Server.SendPackageTo(Target.All, new Package(PackageType.UserListUpdate, Pool.Server.Users));
        }
    }
}