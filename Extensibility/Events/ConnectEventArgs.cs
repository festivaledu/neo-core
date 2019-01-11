using Neo.Core.Networking;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides data for the <see cref="EventType.Connected"/> event.
    /// </summary>
    public class ConnectEventArgs
    {
        public Client Client { get; }

        public ConnectEventArgs(Client client) {
            this.Client = client;
        }
    }
}
