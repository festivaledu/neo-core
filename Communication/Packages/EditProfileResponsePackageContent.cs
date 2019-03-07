using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    public class EditProfileResponsePackageContent
    {
        public Account Account { get; set; }
        public Identity Identity { get; set; }
        public EditProfilePackageContent Request { get; set; }

        public EditProfileResponsePackageContent(Account account, Identity identity, EditProfilePackageContent request = null) {
            this.Account = account;
            this.Identity = identity;
            this.Request = request;
        }
    }
}
