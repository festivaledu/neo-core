using System.Security.Cryptography;

namespace Neo.Core.Cryptography
{
    /// <summary>
    ///     Represents the standard parameters for the <see cref="Aes"/> algorithm.
    /// </summary>
    public struct AesParameters
    {
        /// <summary>
        ///     The key used for the <see cref="Aes"/> algorithm.
        /// </summary>
        public byte[] AesKey { get; }

        /// <summary>
        ///     The initialization vector used for the <see cref="Aes"/> algorithm.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public byte[] AesIV { get; }

        /// <summary>
        ///     Initializes a new structure of <see cref="AesParameters"/> with a key and an initialization vector.
        /// </summary>
        /// <param name="aesKey">The key for this structure.</param>
        /// <param name="aesIV">The initialization vector for this structure.</param>
        // ReSharper disable once InconsistentNaming
        public AesParameters(byte[] aesKey, byte[] aesIV) {
            AesKey = aesKey;
            AesIV = aesIV;
        }

        /// <summary>
        ///     Determines whether this structure contains any data.
        /// </summary>
        /// <returns>Returns <c>true</c> if both <see cref="AesKey"/> and <see cref="AesIV"/> are set, otherwise <c>false</c>.</returns>
        public bool IsValid() => AesKey != null && AesKey.Length != 0 && AesIV != null && AesIV.Length != 0;
    }
}
