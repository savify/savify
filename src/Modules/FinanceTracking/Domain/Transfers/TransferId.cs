using App.BuildingBlocks.Domain;

namespace App.Modules.FinanceTracking.Domain.Transfers;

public class TransferId(Guid value) : TypedIdValueBase(value);
