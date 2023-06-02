namespace App.BuildingBlocks.Domain;

public interface IBusinessRule
{
    bool IsBroken();

    string Message => string.Format(MessageTemplate, MessageArguments ?? Array.Empty<object>());

    string MessageTemplate { get; }

    object[]? MessageArguments => null;
}
