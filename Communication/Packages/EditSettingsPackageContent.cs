namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.EditSettings"/> package.
    /// </summary>
    public class EditSettingsPackageContent
    {
        /// <summary>
        ///     The scope the edited model applies to.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        ///     The edited model.
        /// </summary>
        public dynamic Model { get; set; }
    }
}
