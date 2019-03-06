using System;
using System.Linq;
using Neo.Core.Communication.Packages;
using Neo.Core.Cryptography;
using Neo.Core.Extensibility;
using Neo.Core.Shared;

namespace Neo.Core.Authentication
{
    public static class Authenticator
    {
        private static readonly Random random = new Random();
        private const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public static AuthenticationResult Authenticate(Identity loginData, out Guest guest) {
            do {
                loginData.Id = $"Guest-{new string(Enumerable.Range(1, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray())}";
            } while (Pool.Server.Users.Any(u => u.Identity.Id == loginData.Id));

            guest = new Guest {
                Identity = loginData
            };

            return AuthenticationResult.Success;
        }

        public static AuthenticationResult Authenticate(MemberLoginPackageContent loginData, out Member member) {
            var account = Pool.Server.Accounts.Find(a => a.Email == loginData.User || a.Identity.Id == loginData.User);

            member = null;

            if (account == null) {
                return AuthenticationResult.UnknownUser;
            }

            // TODO: Implement random salt
            if (!account.Password.SequenceEqual(Convert.FromBase64String(loginData.Password))) {
                return AuthenticationResult.IncorrectPassword;
            }

            if (account.Member != null) {
                return AuthenticationResult.ExistingSession;
            }

            member = new Member {
                Account = account,
                Attributes = { ["session.neo.origin"] = "neo.client" }
            };

            return AuthenticationResult.Success;
        }

        public static Member CreateVirtualMember(Plugin parent) {
            var count = Pool.Server.Users.Count(u => u is Member m && m.Attributes.ContainsKey("instance.neo.origin") && m.Attributes["instance.neo.origin"].Equals(parent.InternalId));

            var account = new Account {
                Email = $"{parent.InternalId}-{count}@plugin.neo"
            };

            var member = new Member {
                Account = account,
                Attributes = { ["instance.neo.origin"] = parent.InternalId }
            };

            return member;
        }

        public static AuthenticationResult Register(RegisterPackageContent registerData, out (Account account, Member member)? user) {
            return Register(registerData.Name, registerData.Id, registerData.Email, registerData.Password, out user);
        }

        public static AuthenticationResult Register(string name, string id, string email, string password, out (Account account, Member member)? user) {
            return Register(name, id, email, NeoCryptoProvider.Instance.Sha512ComputeHash(password), out user);
        }

        public static AuthenticationResult Register(string name, string id, string email, byte[] password, out (Account account, Member member)? user) {
            if (Pool.Server.Accounts.Any(a => a.Email == email)) {
                user = null;
                return AuthenticationResult.EmailInUse;
            }

            if (Pool.Server.Accounts.Any(a => a.Identity.Id == id)) {
                user = null;
                return AuthenticationResult.IdInUse;
            }

            var account = new Account {
                Email = email,
                Password = password,
                Identity = new Identity { Id = id, Name = name }
            };

            var member = new Member {
                Account = account
            };

            user = (account, member);
            return AuthenticationResult.Success;
        }
    }
}
