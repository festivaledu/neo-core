namespace Neo.Core.Authentication
{
    public enum AuthenticationResult
    {
        Success,
        UnknownUser,
        IncorrectPassword,
        ExistingSession,
        IdInUse,
        EmailInUse,
    }
}
