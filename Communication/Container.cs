namespace Neo.Core.Communication
{
    public class Container
    {
        public bool IsEncrypted { get; }
        public string Payload { get; set; }

        public Container() { }

        internal Container(bool encrypted, string payload) {
            this.IsEncrypted = encrypted;
            this.Payload = payload;
        }
    }
}
