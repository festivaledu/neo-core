using System.Net;
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
        ///     The <see cref="IPAddress"/> of the <see cref="Networking.Client"/> connecting.
        /// </summary>
        public IPAddress Address { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectEventArgs"/> class.
        /// </summary>
        /// <param name="client">The <see cref="Networking.Client"/> connecting.</param>
        /// <param name="address">The <see cref="IPAddress"/> of the <see cref="Networking.Client"/> connecting.</param>
        public ConnectEventArgs(Client client, IPAddress address) {
            this.Client = client;
            this.Address = address;
        }
    }
}
