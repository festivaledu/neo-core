using System;
using System.Linq;
using Neo.Core.Communication;
using Neo.Core.Shared;

namespace Neo.Core.Authentication
{
    public static class Authenticator
    {
        private static readonly Random random = new Random();
        private const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        public static AuthenticationResult Authenticate(GuestLoginPayload loginData, out Guest guest) {
            do {
                loginData.Identity.Id = $"Guest-{new string(Enumerable.Range(1, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray())}";
            } while (Pool.Server.Users.Any(u => u.Identity.Id == loginData.Identity.Id));

            guest = new Guest {
                Identity = loginData.Identity
            };

            return AuthenticationResult.Success;
        }

        public static AuthenticationResult Authenticate(MemberLoginPayload loginData, out Member member) {
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
                Account = account
            };

            member.Attributes.Add("neo.member.origin", "neo.client");
            return AuthenticationResult.Success;
        }
    }
}
