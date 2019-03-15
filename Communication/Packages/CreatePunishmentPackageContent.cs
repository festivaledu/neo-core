using System;
using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.CreatePunishment"/> package.
    /// </summary>
    public class CreatePunishmentPackageContent
    {
        /// <summary>
        ///     The id of the <see cref="User"/> to punish.
        /// </summary>
        public Guid Target { get; set; }

        /// <summary>
        ///     The action to perform.
        /// </summary>
        public string Action { get; set; }
    }
}
