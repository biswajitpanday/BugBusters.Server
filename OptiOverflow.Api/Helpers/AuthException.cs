namespace BugBusters.Server.Api.Helpers;

public class AuthException : Exception
{
    public AuthException(string message) : base(message) { }
}