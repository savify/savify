namespace App.BuildingBlocks.Application.Queries;

public static class PagedQueryHelper
{
    public const string Limit = "Limit";
    public const string Offset = "Offset";


    public static PageData GetPageData(IPagedQuery query)
    {
        int offset;
        if (!query.Page.HasValue || !query.PerPage.HasValue)
        {
            offset = 0;
        }
        else
        {
            offset = (query.Page.Value - 1) * query.PerPage.Value;
        }

        int limit;
        if (!query.PerPage.HasValue)
        {
            limit = int.MaxValue;
        }
        else
        {
            limit = query.PerPage.Value;
        }

        return new PageData(offset, limit);
    }

    public static string AppendPageStatement(string sql)
    {
        return $"{sql} " + $"LIMIT @{Limit} OFFSET @{Offset}; ";
    }
}
