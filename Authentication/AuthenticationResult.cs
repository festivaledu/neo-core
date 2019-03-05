namespace Neo.Core.Authentication
{
    public enum AuthenticationResult
    {
        Success,
        UnknownUser,
        IncorrectPassword,
        ExistingSession,
        EmailInUse,
    }
}
