namespace Neo.Core.Cryptography
{
    public struct CryptographicData
    {
        public byte[] AesKey { get; }

        // ReSharper disable once InconsistentNaming
        public byte[] AesIV { get; }
        

        // ReSharper disable once InconsistentNaming
        public CryptographicData(byte[] aesKey, byte[] aesIV) {
            AesKey = aesKey;
            AesIV = aesIV;
        }

        public bool IsValid() => AesKey != null && AesKey.Length != 0 && AesIV != null && AesIV.Length != 0;
    }
}
