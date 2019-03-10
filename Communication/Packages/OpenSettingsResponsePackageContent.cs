using System.Collections.Generic;

namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.OpenSettingsResponse"/> package.
    /// </summary>
    public class OpenSettingsResponsePackageContent
    {
        /// <summary>
        ///     The model that represents the opened settings.
        /// </summary>
        public dynamic Model { get; set; }

        /// <summary>
        ///     The human-readable titles for all editable fields.
        /// </summary>
        public Dictionary<string, string> Titles { get; set; } = new Dictionary<string, string>();


        /// <summary>
        ///     Initializes a new instance of the <see cref="OpenSettingsResponsePackageContent"/> class.
        /// </summary>
        /// <param name="model">The model that represents the opened settings.</param>
        public OpenSettingsResponsePackageContent(dynamic model) {
            this.Model = model;
        }
    }
}
