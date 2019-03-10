namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.EditProfile"/> package.
    /// </summary>
    public class EditProfilePackageContent
    {
        /// <summary>
        ///     The key of the field to edit.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     The value to set the field to.
        /// </summary>
        public object Value { get; set; }
    }
}
