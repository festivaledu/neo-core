namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.MemberLogin"/> package.
    /// </summary>
    public class MemberLoginPackageContent
    {
        /// <summary>
        ///     The account id or email entered by the client.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        ///     The password entered by the client.
        /// </summary>
        public string Password { get; set; }
    }
}
