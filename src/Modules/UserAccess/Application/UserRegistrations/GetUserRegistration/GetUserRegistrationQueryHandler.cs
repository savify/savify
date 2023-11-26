using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Configuration.Data;
using App.Modules.UserAccess.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;

internal class GetUserRegistrationQueryHandler : IQueryHandler<GetUserRegistrationQuery, UserRegistrationDto?>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUserRegistrationQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<UserRegistrationDto?> Handle(GetUserRegistrationQuery query, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        var sql = $"""
                   SELECT id, email, name, status, valid_till as validTill
                   FROM {DatabaseConfiguration.Schema.Name}.user_registrations
                   WHERE id = @UserRegistrationId
                   """;

        return await connection.QuerySingleOrDefaultAsync<UserRegistrationDto>(sql, new { query.UserRegistrationId });
    }
}
