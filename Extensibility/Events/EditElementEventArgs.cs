using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
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
