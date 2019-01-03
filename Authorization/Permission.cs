namespace Neo.Core.Authorization
{
    /// <summary>
    ///     Specifies whether a permission is granted.
    /// </summary>
    public enum Permission
    {
        /// <summary>
        ///     The permission is denied.
        /// </summary>
        Deny,
        /// <summary>
        ///     The permission is granted.
        /// </summary>
        Allow,
        /// <summary>
        ///     The permission will be inherited.
        /// </summary>
        Inherit
    }
}
