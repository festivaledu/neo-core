namespace Neo.Core.Shared
{
    /// <summary>
    ///     Represents the public appearance of a <see cref="User"/>.
    /// </summary>
    public class Identity
    {
        /// <summary>
        ///     The user-defined id of the <see cref="User"/>.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The name of the <see cref="User"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The file extension of the avatar of the <see cref="User"/>.
        /// </summary>
        public string AvatarFileExtension { get; set; }
    }
}
