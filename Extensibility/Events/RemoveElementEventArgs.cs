using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides data for all remove events.
    /// </summary>
    /// <typeparam name="TElement">A removable element.</typeparam>
    public class RemoveElementEventArgs<TElement> : ICancellableEvent
    {
        public User Remover { get; }
        public TElement Element { get; }

        public RemoveElementEventArgs(User remover, TElement element) {
            this.Remover = remover;
            this.Element = element;
        }
    }
}
