using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.LoginResponse"/> package.
    /// </summary>
    public class LoginResponsePackageContent
    {
        /// <summary>
        ///     The status of the login or register action.
        /// </summary>
        public int Status { get; }

        /// <summary>
        ///     The identity of the logged in or registered user.
        /// </summary>
        public Identity Identity { get; }

        /// <summary>
        ///     The account of the logged in or registered member.
        /// </summary>
        public Account Account { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginResponsePackageContent"/> class.
        /// </summary>
        /// <param name="status">The status of the login or register action.</param>
        /// <param name="identity">The identity of the logged in or registered user.</param>
        /// <param name="account">The account of the logged in or registered member.</param>
        public LoginResponsePackageContent(int status, Identity identity, Account account = null) {
            this.Status = status;
            this.Identity = identity;
            this.Account = account;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginResponsePackageContent"/> class with the status set to <c>Success</c>.
        /// </summary>
        /// <param name="identity">The identity of the logged in or registered user.</param>
        /// <param name="account">The account of the logged in or registered member.</param>
        /// <returns>Returns the created content.</returns>
        public static LoginResponsePackageContent GetSuccessful(Identity identity, Account account = null) {
            return new LoginResponsePackageContent(0, identity, account);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginResponsePackageContent"/> class with the status set to <c>UnknownUser</c>.
        /// </summary>
        /// <returns>Returns the created content.</returns>
        public static LoginResponsePackageContent GetUnknownUser() {
            return new LoginResponsePackageContent(1, null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginResponsePackageContent"/> class with the status set to <c>IncorrectPassword</c>.
        /// </summary>
        /// <returns>Returns the created content.</returns>
        public static LoginResponsePackageContent GetIncorrectPassword() {
            return new LoginResponsePackageContent(2, null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginResponsePackageContent"/> class with the status set to <c>Unauthorized</c>.
        /// </summary>
        /// <returns>Returns the created content.</returns>
        public static LoginResponsePackageContent GetUnauthorized() {
            return new LoginResponsePackageContent(3, null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginResponsePackageContent"/> class with the status set to <c>IdInUse</c>.
        /// </summary>
        /// <returns>Returns the created content.</returns>
        public static LoginResponsePackageContent GetIdInUse() {
            return new LoginResponsePackageContent(4, null);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginResponsePackageContent"/> class with the status set to <c>EmailInUse</c>.
        /// </summary>
        /// <returns>Returns the created content.</returns>
        public static LoginResponsePackageContent GetEmailInUse() {
            return new LoginResponsePackageContent(5, null);
        }
    }
}
