namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides data for all receive events.
    /// </summary>
    /// <typeparam name="TElement">A receivable element.</typeparam>
    public class ReceiveElementEventArgs<TElement> : ICancellableEvent
    {
        // TODO
        public TElement Element { get; }
    }
}
