using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.CreateChannel"/> package.
    /// </summary>
    public class CreateChannelPackageContent
    {
        /// <summary>
        ///     The name of the <see cref="Channel"/> to create.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The id of the <see cref="Channel"/> to create.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The limit of the <see cref="Channel"/> to create.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        ///     The password of the <see cref="Channel"/> to create.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     The lifetime of the <see cref="Channel"/> to create.
        /// </summary>
        public Lifespan Lifetime { get; set; }
    }
}
