using App.Modules.Categories.Application.Contracts;

namespace App.Modules.Categories.Infrastructure.Configuration.Processing.Outbox;

public class ProcessOutboxCommand : CommandBase, IRecurringCommand;
