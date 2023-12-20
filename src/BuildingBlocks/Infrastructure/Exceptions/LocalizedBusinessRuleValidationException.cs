using App.BuildingBlocks.Domain;

namespace App.BuildingBlocks.Infrastructure.Exceptions;

public class LocalizedBusinessRuleValidationException(IBusinessRule brokenRule, string localizedMessage)
    : BusinessRuleValidationException(brokenRule, localizedMessage);
