using App.Modules.Accounts.Application.Contracts;

namespace App.Modules.Accounts.Application.ViewMetadata.GetViewMetadata;
public class GetAccountViewMetadataQuery : QueryBase<AccountViewMetadataDto>
{
    public Guid AccountId { get; }

    public GetAccountViewMetadataQuery(Guid accountId)
    {
        AccountId = accountId;
    }
}
