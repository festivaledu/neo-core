// ReSharper disable InconsistentNaming

using Neo.Core.Cryptography;
using Neo.Core.Shared;

namespace Neo.Core.Config
{
    public class ConfigValues
    {
        [EditableProperty("Gäste zulassen?")]
        public bool GuestsAllowed { get; set; } = false;

        [EditableProperty("Registrierung zulassen?")]
        public bool RegistrationAllowed { get; set; } = false;
        public Identity RootIdentity { get; set; } = new Identity { Id = "neo", Name = "Neo" };
        public byte[] RootPassword { get; set; } = NeoCryptoProvider.Instance.Sha512ComputeHash("LT6SUK7JBELX3UAL");
        public int RSAKeySize { get; set; } = 4096;
        public string ServerAddress { get; set; } = "0.0.0.0";

        [EditableProperty("Servername")]
        public string ServerName { get; set; } = "Neo Server";
        public int ServerPort { get; set; } = 42420;
    }
}
