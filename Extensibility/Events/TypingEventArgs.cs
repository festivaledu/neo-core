using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides data for the <see cref="EventType.Typing"/> event.
    /// </summary>
    public class TypingEventArgs
    {
        public User User { get; }
        public Channel Channel { get; }
        public string CurrentInput { get; }

        public TypingEventArgs(User user, Channel channel, string currentInput) {
            this.User = user;
            this.Channel = channel;
            this.CurrentInput = currentInput;
        }
    }
}
