namespace OptiOverflow.Api.Helpers;

public class AuthException : Exception
{
    public AuthException(string message) : base(message) { }
}