// ReSharper disable InconsistentNaming

using Neo.Core.Cryptography;
using Neo.Core.Shared;

namespace Neo.Core.Config
{
    public class ConfigValues
    {
        public Identity RootIdentity { get; set; } = new Identity { Id = "neo", Name = "Neo" };
        public byte[] RootPassword { get; set; } = NeoCryptoProvider.Instance.Sha512ComputeHash("LT6SUK7JBELX3UAL");
        public int RSAKeySize { get; set; } = 4096;
        public string ServerAddress { get; set; } = "0.0.0.0";
        public int ServerPort { get; set; } = 42420;
    }
}
