using App.BuildingBlocks.Application.Data;
using App.Modules.UserAccess.Application.Configuration.Queries;
using Dapper;

namespace App.Modules.UserAccess.Application.UserRegistrations.GetUserRegistration;

public class GetUserRegistrationQueryHandler : IQueryHandler<GetUserRegistrationQuery, UserRegistrationDto>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetUserRegistrationQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<UserRegistrationDto> Handle(GetUserRegistrationQuery query, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        string sql = "SELECT id, email, name, status, valid_till as validTill " +
                     "FROM user_access.user_registrations WHERE id = @UserRegistrationId";

        return await connection.QuerySingleAsync<UserRegistrationDto>(sql, new { query.UserRegistrationId });
    }
}
