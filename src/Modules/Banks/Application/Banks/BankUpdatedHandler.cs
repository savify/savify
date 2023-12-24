using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using App.Modules.Banks.Domain.Banks.Events;
using MediatR;

namespace App.Modules.Banks.Application.Banks;

public class BankUpdatedHandler(IBankRepository bankRepository, IBankRevisionRepository bankRevisionRepository) : INotificationHandler<BankUpdatedDomainEvent>
{
    public async Task Handle(BankUpdatedDomainEvent @event, CancellationToken cancellationToken)
    {
        var bank = await bankRepository.GetByIdAsync(@event.BankId);
        var bankRevision = bank.CreateRevision(BankRevisionType.Updated);

        await bankRevisionRepository.AddAsync(bankRevision);
    }
}
