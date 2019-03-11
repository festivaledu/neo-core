using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for the <see cref="EventType.Input"/> event.
    /// </summary>
    public class InputEventArgs : ICancellableEvent
    {
        /// <summary>
        ///     The sender of the input.
        /// </summary>
        public User Sender { get; }

        /// <summary>
        ///     The input sent.
        /// </summary>
        public string Input { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="InputEventArgs"/> class.
        /// </summary>
        /// <param name="sender">The sender of the input.</param>
        /// <param name="input">The input sent.</param>
        public InputEventArgs(User sender, string input) {
            this.Sender = sender;
            this.Input = input;
        }
    }
}
