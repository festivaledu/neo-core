using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides data for all leave events.
    /// </summary>
    /// <typeparam name="TElement">A leavable element.</typeparam>
    public class LeaveElementEventArgs<TElement> : ICancellableEvent
    {
        public User Leaver { get; }
        public TElement Element { get; }

        public LeaveElementEventArgs(User leaver, TElement element) {
            this.Leaver = leaver;
            this.Element = element;
        }
    }
}
