using Neo.Core.Networking;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides data for the <see cref="EventType.Disconnected"/> event.
    /// </summary>
    public class DisconnectEventArgs
    {
        public Client Client { get; }
        public ushort Code { get; }
        public string Reason { get; }
        public bool WasClean { get; }

        public DisconnectEventArgs(Client client, ushort code, string reason, bool wasClean) {
            this.Client = client;
            this.Code = code;
            this.Reason = reason;
            this.WasClean = wasClean;
        }
    }
}
