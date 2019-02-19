using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    public class InputEventArgs : ICancellableEvent
    {
        public User Sender { get; }
        public string Input { get; }

        public InputEventArgs(User sender, string input) {
            this.Sender = sender;
            this.Input = input;
        }
    }
}
