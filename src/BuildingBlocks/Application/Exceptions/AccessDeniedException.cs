namespace App.BuildingBlocks.Application.Exceptions;

public class AccessDeniedException(string messageTemplate = "Access to this resource is forbidden", object[]? messageArguments = null) : Exception
{
    public override string Message => string.Format(messageTemplate, messageArguments ?? Array.Empty<object>());
}
