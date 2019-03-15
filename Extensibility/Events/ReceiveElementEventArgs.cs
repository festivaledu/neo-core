using Neo.Core.Networking;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for all receive events.
    /// </summary>
    /// <typeparam name="TElement">A receivable element.</typeparam>
    public class ReceiveElementEventArgs<TElement> : ICancellableEvent
    {
        /// <summary>
        ///     The sender of the <see cref="TElement"/>.
        /// </summary>
        public Client Sender { get; }

        /// <summary>
        ///     The element received.
        /// </summary>
        public TElement Element { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="ReceiveElementEventArgs{TElement}"/> class.
        /// </summary>
        /// <param name="sender">The sender of the <see cref="TElement"/>.</param>
        /// <param name="element">The received element.</param>
        public ReceiveElementEventArgs(Client sender, TElement element) {
            this.Sender = sender;
            this.Element = element;
        }
    }
}
