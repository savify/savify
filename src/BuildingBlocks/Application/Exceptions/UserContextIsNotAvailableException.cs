namespace App.BuildingBlocks.Application.Exceptions;

public class UserContextIsNotAvailableException : ApplicationException
{
    public UserContextIsNotAvailableException(string? message) : base(message)
    {
    }
}
