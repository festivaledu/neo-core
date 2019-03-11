using Neo.Core.Networking;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for the <see cref="EventType.Disconnected"/> event.
    /// </summary>
    public class DisconnectEventArgs
    {
        /// <summary>
        ///     The <see cref="Client"/> disconnecting.
        /// </summary>
        public Client Client { get; }

        /// <summary>
        ///     The code of the disconnect provided by the socket.
        /// </summary>
        public ushort Code { get; }

        /// <summary>
        ///     The reason of the disconnect provided by the socket.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        ///     Determines whether the disconnect was clean.
        /// </summary>
        public bool WasClean { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="DisconnectEventArgs"/> class.
        /// </summary>
        /// <param name="client">The <see cref="Client"/> disconnecting.</param>
        /// <param name="code">The code of the disconnect provided by the socket.</param>
        /// <param name="reason">The reason of the disconnect provided by the socket.</param>
        /// <param name="wasClean">Whether the disconnect was clean.</param>
        public DisconnectEventArgs(Client client, ushort code, string reason, bool wasClean) {
            this.Client = client;
            this.Code = code;
            this.Reason = reason;
            this.WasClean = wasClean;
        }
    }
}
