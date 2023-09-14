using Dapper;

namespace App.BuildingBlocks.Application.Queries;

public class PagedSqlQueryBuilder
{
    private const string Limit = "Limit";

    private const string Offset = "Offset";

    private PagedSqlQuery? _pagedSqlQuery;

    private bool _isReadyToBuild;

    public PagedSqlQueryBuilder WithSql(string sql)
    {
        _pagedSqlQuery = new PagedSqlQuery(AppendPageStatement(sql));

        return this;
    }

    public PagedSqlQueryBuilder WithParametersFrom(IPagedQuery query)
    {
        if (_pagedSqlQuery is null)
        {
            throw new InvalidOperationException("Sql was not set. Use \"WithSql\" method first");
        }

        _pagedSqlQuery.Parameters.Add(nameof(Offset), GetPageOffset(query));
        _pagedSqlQuery.Parameters.Add(nameof(Limit), GetPageLimit(query));
        _isReadyToBuild = true;

        return this;
    }

    public PagedSqlQuery Build()
    {
        if (_pagedSqlQuery is null)
        {
            throw new InvalidOperationException("Sql was not set. Use \"WithSql()\" method first");
        }

        if (!_isReadyToBuild)
        {
            throw new InvalidOperationException("Parameters were not set. Use \"WithParametersFrom()\" method first");
        }

        return _pagedSqlQuery;
    }

    private static int GetPageOffset(IPagedQuery query)
    {
        var offset = 0;
        if (query.Page.HasValue && query.PerPage.HasValue)
        {
            offset = (query.Page.Value - 1) * query.PerPage.Value;
        }

        return offset;
    }

    private static int GetPageLimit(IPagedQuery query) => query.PerPage ?? int.MaxValue;

    private static string AppendPageStatement(string sql) => $"{sql} " + $"LIMIT @{Limit} OFFSET @{Offset}; ";

    public class PagedSqlQuery
    {
        public string Sql { get; set; }

        public DynamicParameters Parameters { get; set; }

        public PagedSqlQuery(string sql)
        {
            Sql = sql;
            Parameters = new DynamicParameters();
        }
    }
}
