using App.Modules.FinanceTracking.Application.Contracts;

namespace App.Modules.FinanceTracking.Infrastructure.Configuration.Processing.Inbox;

public class ProcessInboxCommand : CommandBase, IRecurringCommand;
