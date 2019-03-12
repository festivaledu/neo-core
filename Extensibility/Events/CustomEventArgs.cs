using System;

namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Provides the data for the <see cref="EventType.Custom"/> event.
    /// </summary>
    public class CustomEventArgs
    {
        /// <summary>
        ///     The name of this <see cref="CustomEventArgs"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The internal id of the sender of this <see cref="CustomEventArgs"/>.
        /// </summary>
        public Guid Sender { get; set; }

        /// <summary>
        ///     The content of this <see cref="CustomEventArgs"/>.
        /// </summary>
        public object[] Content { get; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomEventArgs"/> class.
        /// </summary>
        /// <param name="name">The name of this <see cref="CustomEventArgs"/>.</param>
        /// <param name="sender">The internal id of the sender of this <see cref="CustomEventArgs"/>.</param>
        /// <param name="content">The content of this <see cref="CustomEventArgs"/>.</param>
        public CustomEventArgs(string name, Guid sender, params object[] content) {
            this.Name = name;
            this.Sender = sender;
            this.Content = content;
        }
    }
}
