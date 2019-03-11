using Neo.Core.Networking;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for the <see cref="EventType.Connected"/> event.
    /// </summary>
    public class ConnectEventArgs
    {
        /// <summary>
        ///     The <see cref="Networking.Client"/> connecting.
        /// </summary>
        public Client Client { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectEventArgs"/> class.
        /// </summary>
        /// <param name="client"> The <see cref="Networking.Client"/> connecting.</param>
        public ConnectEventArgs(Client client) {
            this.Client = client;
        }
    }
}
