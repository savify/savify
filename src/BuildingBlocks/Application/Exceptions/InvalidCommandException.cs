namespace App.BuildingBlocks.Application.Exceptions;

public class InvalidCommandException : Exception
{
    public Dictionary<string, List<string>> Errors { get; }

    public InvalidCommandException(Dictionary<string, List<string>> errors)
    {
        Errors = errors;
    }
}
