namespace Core.exceptions;

public class NetworkException : Exception
{
    public NetworkException(string? message) : base($"Error during networking: {message}")
    {
    }
}