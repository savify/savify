using App.BuildingBlocks.Application.Queries;
using App.Modules.Banks.Application.Contracts;

namespace App.Modules.Banks.Application.Banks.Public.GetBanks;

public class GetBanksQuery : QueryBase<List<BankDto>>, IPagedQuery
{
    public int? Page { get; }

    public int? PerPage { get; }

    public GetBanksQuery(int? page = null, int? perPage = null)
    {
        Page = page;
        PerPage = perPage;
    }
}
