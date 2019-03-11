using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for all join events.
    /// </summary>
    /// <typeparam name="TElement">A joinable element.</typeparam>
    public class JoinElementEventArgs<TElement> : ICancellableEvent
    {
        /// <summary>
        ///     The <see cref="User"/> joining the <see cref="TElement"/>.
        /// </summary>
        public User Joiner { get; }

        /// <summary>
        ///     The element the <see cref="User"/> joined.
        /// </summary>
        public TElement Element { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="JoinElementEventArgs{TElement}"/> class.
        /// </summary>
        /// <param name="joiner">The <see cref="User"/> joining the <see cref="TElement"/>.</param>
        /// <param name="element">The element the <see cref="User"/> joined.</param>
        public JoinElementEventArgs(User joiner, TElement element) {
            this.Joiner = joiner;
            this.Element = element;
        }
    }
}
