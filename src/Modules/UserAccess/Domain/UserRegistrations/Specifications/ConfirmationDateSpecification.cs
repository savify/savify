using App.BuildingBlocks.Domain.Specifications;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Specifications;

public class ConfirmationDateSpecification : CompositeSpecification<DateTime>
{
    private static readonly TimeSpan ValidTimeSpan = new(0, 30, 0);

    public override bool IsSatisfiedBy(DateTime registrationDate)
    {
        return registrationDate.Add(ValidTimeSpan) > DateTime.UtcNow;
    }
}
