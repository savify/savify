using System.Data;
using App.BuildingBlocks.Application.Data;
using Npgsql;

namespace App.BuildingBlocks.Infrastructure.Data;

public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
{
    private readonly string _connectionString;
    
    private IDbConnection? _connection;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IDbConnection GetOpenConnection()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            _connection = CreateNewConnection();
        }

        return _connection;
    }

    public IDbConnection CreateNewConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return connection;
    }

    public string GetConnectionString()
    {
        return _connectionString;
    }

    public void Dispose()
    {
        if (_connection != null && _connection.State == ConnectionState.Open)
        {
            _connection.Dispose();
        }
    }
}
