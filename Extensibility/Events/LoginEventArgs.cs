using Neo.Core.Shared;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for the <see cref="EventType.Login"/> event.
    /// </summary>
    public class LoginEventArgs
    {
        /// <summary>
        ///     The <see cref="Shared.User"/> who logged in.
        /// </summary>
        public User User { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginEventArgs"/> class.
        /// </summary>
        /// <param name="user">The <see cref="Shared.User"/> who logged in.</param>
        public LoginEventArgs(User user) {
            this.User = user;
        }
    }
}
