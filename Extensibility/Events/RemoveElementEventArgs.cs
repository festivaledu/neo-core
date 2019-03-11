using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for all remove events.
    /// </summary>
    /// <typeparam name="TElement">A removable element.</typeparam>
    public class RemoveElementEventArgs<TElement> : ICancellableEvent
    {
        /// <summary>
        ///     The <see cref="User"/> removing the <see cref="TElement"/>.
        /// </summary>
        public User Remover { get; }

        /// <summary>
        ///     The element to remove.
        /// </summary>
        public TElement Element { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoveElementEventArgs{TElement}"/> class.
        /// </summary>
        /// <param name="remover">The <see cref="User"/> removing the <see cref="TElement"/>.</param>
        /// <param name="element">The element to remove.</param>
        public RemoveElementEventArgs(User remover, TElement element) {
            this.Remover = remover;
            this.Element = element;
        }
    }
}
