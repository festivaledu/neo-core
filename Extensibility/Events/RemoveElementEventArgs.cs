using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
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
