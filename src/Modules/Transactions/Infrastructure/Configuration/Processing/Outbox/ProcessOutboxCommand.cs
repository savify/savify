using App.Modules.Transactions.Application.Contracts;

namespace App.Modules.Transactions.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommand : CommandBase, IRecurringCommand;
