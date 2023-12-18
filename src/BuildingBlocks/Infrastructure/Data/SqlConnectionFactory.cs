using System.Data;
using App.BuildingBlocks.Application.Data;
using Npgsql;

namespace App.BuildingBlocks.Infrastructure.Data;

public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory, IDisposable
{
    private IDbConnection? _connection;

    public IDbConnection GetOpenConnection()
    {
        if (_connection is not { State: ConnectionState.Open })
        {
            _connection = CreateNewConnection();
        }

        return _connection;
    }

    public IDbConnection CreateNewConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        return connection;
    }

    public string GetConnectionString()
    {
        return connectionString;
    }

    public void Dispose()
    {
        if (_connection is { State: ConnectionState.Open })
        {
            _connection.Dispose();
        }
    }
}
