using System.Security.Cryptography;

namespace Neo.Core.Cryptography
{
    /// <summary>
    ///     Represents the standard parameters for the <see cref="Aes"/> algorithm.
    /// </summary>
    public struct AesParameters
    {
        public byte[] AesKey { get; }

        // ReSharper disable once InconsistentNaming
        public byte[] AesIV { get; }
        
        // ReSharper disable once InconsistentNaming
        public AesParameters(byte[] aesKey, byte[] aesIV) {
            AesKey = aesKey;
            AesIV = aesIV;
        }

        public bool IsValid() => AesKey != null && AesKey.Length != 0 && AesIV != null && AesIV.Length != 0;
    }
}
