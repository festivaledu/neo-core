using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.EditProfileResponse"/> package.
    /// </summary>
    public class EditProfileResponsePackageContent
    {
        /// <summary>
        ///     The account of the edited profile.
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        ///     The identity of the edited profile.
        /// </summary>
        public Identity Identity { get; set; }

        /// <summary>
        ///     The original edit request.
        /// </summary>
        public EditProfilePackageContent Request { get; set; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="EditProfileResponsePackageContent"/> class.
        /// </summary>
        /// <param name="account">The account of the edited profile.</param>
        /// <param name="identity">The identity of the edited profile.</param>
        /// <param name="request">The original edit request.</param>
        public EditProfileResponsePackageContent(Account account, Identity identity, EditProfilePackageContent request = null) {
            this.Account = account;
            this.Identity = identity;
            this.Request = request;
        }
    }
}
