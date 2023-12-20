namespace App.BuildingBlocks.Domain;

public static class BusinessRuleChecker
{
    public static void CheckRules(params IBusinessRule[] rules)
    {
        foreach (var rule in rules)
        {
            CheckRule(rule);
        }
    }

    private static void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }
}
