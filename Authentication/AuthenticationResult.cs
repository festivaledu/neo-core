namespace Neo.Core.Authentication
{
    public enum AuthenticationResult
    {
        Success,
        UnknownEmail,
        IncorrectPassword,
        ExistingSession,
        EmailInUse,
    }
}
