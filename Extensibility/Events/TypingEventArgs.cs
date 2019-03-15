using System;
using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for the <see cref="EventType.Typing"/> event.
    /// </summary>
    [Obsolete]
    public class TypingEventArgs
    {
        /// <summary>
        ///     The <see cref="Shared.User"/> typing. 
        /// </summary>
        public User User { get; }

        /// <summary>
        ///     The <see cref="Channel"/> in which this <see cref="TypingEventArgs"/> was invoked.
        /// </summary>
        public Channel Channel { get; }

        /// <summary>
        ///     The input of the <see cref="Shared.User"/> so far.
        /// </summary>
        public string CurrentInput { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="TypingEventArgs"/> class.
        /// </summary>
        /// <param name="user">The <see cref="Shared.User"/> typing.</param>
        /// <param name="channel">The <see cref="Channel"/> in which this <see cref="TypingEventArgs"/> was invoked.</param>
        /// <param name="currentInput">The input of the <see cref="Shared.User"/> so far.</param>
        public TypingEventArgs(User user, Channel channel, string currentInput) {
            this.User = user;
            this.Channel = channel;
            this.CurrentInput = currentInput;
        }
    }
}
