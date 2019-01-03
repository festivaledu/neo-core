namespace Neo.Core.Extensibility.Events
{
    public class ReceiveElementEventArgs<TElement> : ICancellableEvent
    {
        // TODO
        public TElement Element { get; }
    }
}
