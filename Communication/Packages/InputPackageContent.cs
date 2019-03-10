using System;
using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.Input"/> package.
    /// </summary>
    public class InputPackageContent
    {
        /// <summary>
        ///     The input entered by the client.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        ///     The id of the <see cref="Channel"/> this input targets.
        /// </summary>
        public Guid TargetChannel { get; set; }
    }
}
