using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for all create events.
    /// </summary>
    /// <typeparam name="TElement">A creatable element.</typeparam>
    public class CreateElementEventArgs<TElement> : ICancellableEvent
    {
        /// <summary>
        ///     The <see cref="User"/> creating the <see cref="TElement"/>.
        /// </summary>
        public User Creator { get; }

        /// <summary>
        ///     The element created by the <see cref="User"/>.
        /// </summary>
        public TElement Element { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateElementEventArgs{TElement}"/> class.
        /// </summary>
        /// <param name="creator">The <see cref="User"/> creating the <see cref="TElement"/>.</param>
        /// <param name="element">The element created by the <see cref="User"/>.</param>
        public CreateElementEventArgs(User creator, TElement element) {
            this.Creator = creator;
            this.Element = element;
        }
    }
}
