namespace Neo.Core.Extensibility.Events
{
    public class Before<TEvent> where TEvent : ICancellableEvent
    {
        public bool Cancel { get; set; }
        public TEvent Event { get; }
        
        public Before(TEvent @event) {
            this.Event = @event;
        }
    }
}
