using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for all edit events.
    /// </summary>
    /// <typeparam name="TElement">An editable element.</typeparam>
    public class EditElementEventArgs<TElement> : ICancellableEvent
    {
        /// <summary>
        ///     The <see cref="User"/> editing the <see cref="TElement"/>.
        /// </summary>
        public User Editor { get; }

        /// <summary>
        ///     The element the <see cref="User"/> edited.
        /// </summary>
        public TElement Element { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="EditElementEventArgs{TElement}"/> class.
        /// </summary>
        /// <param name="editor">The <see cref="User"/> editing the <see cref="TElement"/>.</param>
        /// <param name="element">The element the <see cref="User"/> edited.</param>
        public EditElementEventArgs(User editor, TElement element) {
            this.Editor = editor;
            this.Element = element;
        }
    }
}
