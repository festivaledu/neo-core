using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides data for all edit events.
    /// </summary>
    /// <typeparam name="TElement">An editable element.</typeparam>
    public class EditElementEventArgs<TElement> : ICancellableEvent
    {
        public User Editor { get; }
        public TElement Element { get; }

        public EditElementEventArgs(User editor, TElement element) {
            this.Editor = editor;
            this.Element = element;
        }
    }
}
