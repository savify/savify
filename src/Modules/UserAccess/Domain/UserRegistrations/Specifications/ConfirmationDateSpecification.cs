using App.BuildingBlocks.Domain.Specifications;

namespace App.Modules.UserAccess.Domain.UserRegistrations.Specifications;

public class ConfirmationDateSpecification : CompositeSpecification<DateTime>
{
    public override bool IsSatisfiedBy(DateTime validTill)
    {
        return validTill > DateTime.UtcNow;
    }
}
