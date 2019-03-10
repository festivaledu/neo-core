using Neo.Core.Shared;

namespace Neo.Core.Communication.Packages
{
    /// <summary>
    ///     Contains the data for the <see cref="PackageType.CreateGroup"/> package.
    /// </summary>
    public class CreateGroupPackageContent
    {
        /// <summary>
        ///     The name of the <see cref="Group"/> to create.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The id of the <see cref="Group"/> to create.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     The sort value of the <see cref="Group"/> to create.
        /// </summary>
        public int SortValue { get; set; }
    }
}
