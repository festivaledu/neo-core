namespace Neo.Core.Communication
{
    /// <summary>
    ///     Encapsulates a <see cref="Package"/>.
    /// </summary>
    public class Container
    {
        /// <summary>
        ///     Determines whether or not <see cref="Payload"/> is encrypted.
        /// </summary>
        public bool IsEncrypted { get; set; }

        /// <summary>
        ///     A JSON encoded <see cref="Package"/>.<para>The value of this field is encrypted using the AES algorithm if <see cref="IsEncrypted"/> is <c>true</c>.</para>
        /// </summary>
        public string Payload { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Container"/> class.
        /// </summary>
        public Container() { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Container"/> class with a payload.
        /// </summary>
        /// <param name="encrypted">Whether or not the payload is encrypted.</param>
        /// <param name="payload">A JSON encoded (and optionally AES encrypted) <see cref="Package"/>.</param>
        internal Container(bool encrypted, string payload) {
            this.IsEncrypted = encrypted;
            this.Payload = payload;
        }
    }
}
