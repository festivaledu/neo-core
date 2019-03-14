// ReSharper disable InconsistentNaming

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
        ///     The address the server is listening to.
        /// </summary>
        public string ServerAddress { get; set; } = "0.0.0.0";

        /// <summary>
        ///     The name of the server.
        /// </summary>
        [EditableProperty("Servername")]
        public string ServerName { get; set; } = "neoChat Server";

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
