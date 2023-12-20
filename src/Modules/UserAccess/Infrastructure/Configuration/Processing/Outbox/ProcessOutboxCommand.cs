using App.Modules.UserAccess.Application.Contracts;

namespace App.Modules.UserAccess.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommand : CommandBase, IRecurringCommand;
