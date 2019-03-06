using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    public class LoginResponsePackageContent
    {
        public int Status { get; }
        public Identity Identity { get; }
        public Account Account { get; }

        public LoginResponsePackageContent(int status, Identity identity, Account account = null) {
            this.Status = status;
            this.Identity = identity;
            this.Account = account;
        }

        public static LoginResponsePackageContent GetSuccessful(Identity identity, Account account = null) {
            return new LoginResponsePackageContent(0, identity, account);
        }

        public static LoginResponsePackageContent GetUnknownUser() {
            return new LoginResponsePackageContent(1, null);
        }

        public static LoginResponsePackageContent GetIncorrectPassword() {
            return new LoginResponsePackageContent(2, null);
        }

        public static LoginResponsePackageContent GetUnauthorized() {
            return new LoginResponsePackageContent(3, null);
        }

        public static LoginResponsePackageContent GetIdInUse() {
            return new LoginResponsePackageContent(4, null);
        }

        public static LoginResponsePackageContent GetEmailInUse() {
            return new LoginResponsePackageContent(5, null);
        }
    }
}
