namespace App.BuildingBlocks.Domain;

public class BusinessRuleValidationException(IBusinessRule brokenRule, string? message = null) : Exception(message ?? brokenRule.Message)
{
    public IBusinessRule BrokenRule { get; } = brokenRule;

    public override string ToString()
    {
        return $"{BrokenRule.GetType().FullName}: {Message}";
    }
}
