namespace Neo.Core.Authentication
{
    /// <summary>
    ///     Specifies the result of an authentication action.
    /// </summary>
    public enum AuthenticationResult
    {
        /// <summary>
        ///     The authentication was successful.
        /// </summary>
        Success,
        /// <summary>
        ///     The user specified couldn't be found.
        /// </summary>
        UnknownUser,
        /// <summary>
        ///     The password entered by the user is incorrect.
        /// </summary>
        IncorrectPassword,
        /// <summary>
        ///     The user is already logged in.
        /// </summary>
        ExistingSession,
        /// <summary>
        ///     The user tries to register an account with an already used id.
        /// </summary>
        IdInUse,
        /// <summary>
        ///     The user tries to register an account with an already used email address.
        /// </summary>
        EmailInUse,
    }
}
