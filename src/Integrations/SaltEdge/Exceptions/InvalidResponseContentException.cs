namespace App.Integrations.SaltEdge.Exceptions;

public class InvalidResponseContentException : Exception
{
    public InvalidResponseContentException(string? message) : base(message)
    {
    }
}
