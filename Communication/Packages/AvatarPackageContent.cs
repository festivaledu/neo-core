namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.SetAvatar"/> package.
    /// </summary>
    public class AvatarPackageContent
    {
        /// <summary>
        ///     The avatar uploaded by the client.
        /// </summary>
        public byte[] Avatar { get; set; }

        /// <summary>
        ///     The extension of the original file.
        /// </summary>
        public string FileExtension { get; set; }
    }
}
