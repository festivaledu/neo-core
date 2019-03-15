using System;
using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.EnterChannel"/> package.
    /// </summary>
    public class EnterChannelPackageContent
    {
        /// <summary>
        ///     The id of the <see cref="Channel"/> to enter.
        /// </summary>
        public Guid ChannelId { get; set; }

        /// <summary>
        ///     The password entered by the client.
        /// </summary>
        public string Password { get; set; }
    }
}
