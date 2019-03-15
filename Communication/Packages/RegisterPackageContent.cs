namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.Register"/> package.
    /// </summary>
    public class RegisterPackageContent
    {
        /// <summary>
        ///     The name of the account to register.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The id of the account to register.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The email address of the account to register.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     The password of the account to register.
        /// </summary>
        public byte[] Password { get; set; }
    }
}
