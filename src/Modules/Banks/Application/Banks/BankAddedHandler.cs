using App.Modules.Banks.Domain.Banks;
using App.Modules.Banks.Domain.Banks.BankRevisions;
using App.Modules.Banks.Domain.Banks.Events;
using MediatR;

namespace App.Modules.Banks.Application.Banks;

public class BankAddedHandler : INotificationHandler<BankAddedDomainEvent>
{
    private readonly IBankRepository _bankRepository;

    private readonly IBankRevisionRepository _bankRevisionRepository;

    public BankAddedHandler(IBankRepository bankRepository, IBankRevisionRepository bankRevisionRepository)
    {
        _bankRepository = bankRepository;
        _bankRevisionRepository = bankRevisionRepository;
    }

    public async Task Handle(BankAddedDomainEvent @event, CancellationToken cancellationToken)
    {
        var bank = await _bankRepository.GetByIdAsync(@event.BankId);
        var bankRevision = bank.CreateRevision(BankRevisionType.Added);

        await _bankRevisionRepository.AddAsync(bankRevision);
    }
}
