using Neo.Core.Networking;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides data for all receive events.
    /// </summary>
    /// <typeparam name="TElement">A receivable element.</typeparam>
    public class ReceiveElementEventArgs<TElement> : ICancellableEvent
    {
        // TODO
        public Client Sender { get; }
        public TElement Element { get; }

        public ReceiveElementEventArgs(Client sender, TElement element) {
            this.Sender = sender;
            this.Element = element;
        }
    }
}
