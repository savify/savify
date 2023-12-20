using App.BuildingBlocks.Application.Queries;
using App.Modules.Banks.Application.Contracts;

namespace App.Modules.Banks.Application.Banks.Internal.GetBanks;

public class GetBanksQuery : QueryBase<IList<BankDto>>, IPagedQuery
{
    public int? Page { get; }

    public int? PerPage { get; }

    public GetBanksQuery(int? page = null, int? perPage = null)
    {
        Page = page;
        PerPage = perPage;
    }
}
