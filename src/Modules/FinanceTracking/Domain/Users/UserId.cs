using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Users;

public class UserId(Guid value) : TypedIdValueBase(value);
