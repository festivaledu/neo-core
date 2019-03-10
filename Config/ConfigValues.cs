// ReSharper disable InconsistentNaming

using System;
using Neo.Core.Cryptography;
using Neo.Core.Shared;

namespace Neo.Core.Config
{
    /// <summary>
    ///     Represents the configuration of the server.
    /// </summary>
    public class ConfigValues
    {
        /// <summary>
        ///     Determines whether guests are allowed.
        /// </summary>
        [EditableProperty("Gäste zulassen")]
        public bool GuestsAllowed { get; set; } = false;

        /// <summary>
        ///     Determines whether registration is allowed.
        /// </summary>
        [EditableProperty("Registrierung zulassen")]
        public bool RegistrationAllowed { get; set; } = true;

        /// <summary>
        ///     The identity for the root account.
        /// </summary>
        public Identity RootIdentity { get; set; } = new Identity { Id = "neo", Name = "Neo" };

        /// <summary>
        ///     The password for the root account.
        /// </summary>
        public byte[] RootPassword { get; set; } = NeoCryptoProvider.Instance.Sha512ComputeHash("LT6SUK7JBELX3UAL");

        /// <summary>
        ///     The key size used for RSA encryption.
        /// </summary>
        [Obsolete]
        public int RSAKeySize { get; set; } = 4096;

        /// <summary>
        ///     The address the server is listening to.
        /// </summary>
        public string ServerAddress { get; set; } = "0.0.0.0";

        /// <summary>
        ///     The name of the server.
        /// </summary>
        [EditableProperty("Servername")]
        public string ServerName { get; set; } = "Neo Server";

        /// <summary>
        ///     The port the server is listening on.
        /// </summary>
        public int ServerPort { get; set; } = 42420;

        /// <summary>
        ///     The amount of messages each <see cref="Channel"/> saves.
        /// </summary>
        [EditableProperty("Maximale Länge der Nachrichtenhistorie")]
        public int MessageHistoryLimit { get; set; } = 50;

        /// <summary>
        ///     Determines whether the <see cref="Channel"/>s should save system messages.
        /// </summary>
        [EditableProperty("Systemnachrichten speichern")]
        public bool SaveSystemMessages { get; set; } = false;
    }
}
