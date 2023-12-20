using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using App.Modules.Banks.Domain.Banks.Events;
using MediatR;

namespace App.Modules.Banks.Application.Banks;

public class BankAddedHandler(IBankRepository bankRepository, IBankRevisionRepository bankRevisionRepository) : INotificationHandler<BankAddedDomainEvent>
{
    public async Task Handle(BankAddedDomainEvent @event, CancellationToken cancellationToken)
    {
        var bank = await bankRepository.GetByIdAsync(@event.BankId);
        var bankRevision = bank.CreateRevision(BankRevisionType.Added);

        await bankRevisionRepository.AddAsync(bankRevision);
    }
}
