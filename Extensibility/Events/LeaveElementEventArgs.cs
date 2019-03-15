using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for all leave events.
    /// </summary>
    /// <typeparam name="TElement">A leavable element.</typeparam>
    public class LeaveElementEventArgs<TElement> : ICancellableEvent
    {
        /// <summary>
        ///     The <see cref="User"/> leaving the <see cref="TElement"/>.
        /// </summary>
        public User Leaver { get; }

        /// <summary>
        ///     The element the <see cref="User"/> left.
        /// </summary>
        public TElement Element { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="LeaveElementEventArgs{TElement}"/> class.
        /// </summary>
        /// <param name="leaver">The <see cref="User"/> leaving the <see cref="TElement"/>.</param>
        /// <param name="element">The element the <see cref="User"/> left.</param>
        public LeaveElementEventArgs(User leaver, TElement element) {
            this.Leaver = leaver;
            this.Element = element;
        }
    }
}
