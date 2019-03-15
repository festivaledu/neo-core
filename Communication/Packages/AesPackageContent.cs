namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.AES"/> package.
    /// </summary>
    public class AesPackageContent
    {
        /// <summary>
        ///     The aes key of the client.
        /// </summary>
        public string AesKey { get; set; }

        /// <summary>
        ///     The aes iv of the client.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string AesIV { get; set; }
    }
}
