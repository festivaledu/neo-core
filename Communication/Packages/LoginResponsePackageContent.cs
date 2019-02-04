using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    public class LoginResponsePackageContent
    {
        public int Status { get; }
        public Identity Identity { get; }

        public LoginResponsePackageContent(int status, Identity identity) {
            this.Status = status;
            this.Identity = identity;
        }

        public static LoginResponsePackageContent GetSuccessful(Identity identity) {
            return new LoginResponsePackageContent(0, identity);
        }

        public static LoginResponsePackageContent GetUnknownUser() {
            return new LoginResponsePackageContent(1, null);
        }

        public static LoginResponsePackageContent GetWrongPassword() {
            return new LoginResponsePackageContent(2, null);
        }

        public static LoginResponsePackageContent GetUnauthorized() {
            return new LoginResponsePackageContent(3, null);
        }
    }
}
