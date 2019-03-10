namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.MetaResponse"/> package.
    /// </summary>
    public class ServerMetaResponsePackageContent
    {
        /// <summary>
        ///     The name of the server.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Determines whether guests are allowed on this server.
        /// </summary>
        public bool GuestsAllowed { get; set; }

        /// <summary>
        ///     Determines whether clients can register new accounts.
        /// </summary>
        public bool RegistrationAllowed { get; set; }
    }
}
