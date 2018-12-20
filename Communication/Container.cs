namespace Neo.Core.Communication
{
    internal class Container
    {
        internal bool IsEncrypted { get; }
        internal string Payload { get; set; }

        internal Container(bool encrypted, string payload) {
            this.IsEncrypted = encrypted;
            this.Payload = payload;
        }
    }
}
