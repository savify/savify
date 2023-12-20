using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommand : CommandBase, IRecurringCommand;
