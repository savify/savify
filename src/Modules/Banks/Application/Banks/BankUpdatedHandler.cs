using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using App.Modules.Banks.Domain.Banks.Events;
using MediatR;

namespace App.Modules.Banks.Application.Banks;

public class BankUpdatedHandler : INotificationHandler<BankUpdatedDomainEvent>
{
    private readonly IBankRepository _bankRepository;

    private readonly IBankRevisionRepository _bankRevisionRepository;

    public BankUpdatedHandler(IBankRepository bankRepository, IBankRevisionRepository bankRevisionRepository)
    {
        _bankRepository = bankRepository;
        _bankRevisionRepository = bankRevisionRepository;
    }

    public async Task Handle(BankUpdatedDomainEvent @event, CancellationToken cancellationToken)
    {
        var bank = await _bankRepository.GetByIdAsync(@event.BankId);
        var bankRevision = bank.CreateRevision(BankRevisionType.Updated);

        await _bankRevisionRepository.AddAsync(bankRevision);
    }
}
