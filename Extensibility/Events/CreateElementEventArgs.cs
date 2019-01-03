using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides data for all create events.
    /// </summary>
    /// <typeparam name="TElement">A creatable element.</typeparam>
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
