using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    public class CreateElementEventArgs<TElement> : ICancellableEvent
    {
        public User Creator { get; }
        public TElement Element { get; }

        public CreateElementEventArgs(User creator, TElement element) {
            this.Creator = creator;
            this.Element = element;
        }
    }
}
