using System;
using System.Linq;
using Neo.Core.Communication;
using Neo.Core.Cryptography;
using Neo.Core.Extensibility;
using Neo.Core.Shared;

namespace Neo.Core.Authentication
{
    public static class Authenticator
    {
        private static readonly Random random = new Random();
        private const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public static AuthenticationResult Authenticate(GuestLoginPackageContent loginData, out Guest guest) {
            do {
                loginData.Identity.Id = $"Guest-{new string(Enumerable.Range(1, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray())}";
            } while (Pool.Server.Users.Any(u => u.Identity.Id == loginData.Identity.Id));

            guest = new Guest {
                Identity = loginData.Identity
            };

            return AuthenticationResult.Success;
        }

        public static AuthenticationResult Authenticate(MemberLoginPackageContent loginData, out Member member) {
            var account = Pool.Server.Accounts.Find(a => a.Email == loginData.Email);

            if (account == null) {
                member = null;
                return AuthenticationResult.UnknownEmail;
            }

            // TODO: Implement random salt
            if (!account.Password.SequenceEqual(Convert.FromBase64String(loginData.Password))) {
                member = null;
                return AuthenticationResult.IncorrectPassword;
            }

            if (account.Member != null) {
                member = null;
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

        public static AuthenticationResult Register(string email, string password, out (Account account, Member member)? user) {
            if (Pool.Server.Accounts.Any(a => a.Email == email)) {
                user = null;
                return AuthenticationResult.EmailInUse;
            }

            var account = new Account {
                Email = email,
                Password = NeoCryptoProvider.Instance.Sha512ComputeHash(password)
            };

            var member = new Member {
                Account = account
            };

            user = (account, member);
            return AuthenticationResult.Success;
        }
    }
}
