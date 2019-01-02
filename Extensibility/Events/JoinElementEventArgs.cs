using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    public class JoinElementEventArgs<TElement> : ICancellableEvent
    {
        public User Joiner { get; }
        public TElement Element { get; }

        public JoinElementEventArgs(User joiner, TElement element) {
            this.Joiner = joiner;
            this.Element = element;
        }
    }
}
