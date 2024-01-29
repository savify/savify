namespace App.BuildingBlocks.Application.Exceptions;

public class InvalidCommandException(Dictionary<string, List<string>> errors) : Exception
{
    public Dictionary<string, List<string>> Errors { get; } = errors;
}
