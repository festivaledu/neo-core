namespace Neo.Core.Extensibility.Events
{
    /// <summary>
    ///     Represents an event that is about to happen and still cancellable.
    /// </summary>
    /// <typeparam name="TEvent">A cancellable event.</typeparam>
    public class Before<TEvent> where TEvent : ICancellableEvent
    {
        /// <summary>
        ///     Determines whether <see cref="TEvent"/> should be cancelled.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        ///     The event about to happen.
        /// </summary>
        public TEvent Event { get; }
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="Before{TEvent}"/> class with the given <see cref="TEvent"/>.
        /// </summary>
        /// <param name="event">The event about to happen.</param>
        public Before(TEvent @event) {
            this.Event = @event;
        }
    }
}
