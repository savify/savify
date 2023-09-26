namespace App.Modules.Banks.Domain.Banks.BankRevisions;

public interface IBankRevisionRepository
{
    public Task AddAsync(BankRevision bankRevision);

    public Task<BankRevision> GetByIdAsync(BankRevisionId bankRevisionId);
}
