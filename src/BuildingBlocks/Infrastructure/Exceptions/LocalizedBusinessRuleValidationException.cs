using App.BuildingBlocks.Domain;

namespace App.BuildingBlocks.Infrastructure.Exceptions;

public class LocalizedBusinessRuleValidationException : BusinessRuleValidationException
{
    public LocalizedBusinessRuleValidationException(IBusinessRule brokenRule, string localizedMessage) : base(brokenRule, localizedMessage)
    {
    }
}
