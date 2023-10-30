using System.Reflection;

namespace App.Database.Scripts.Clear;

public static class ClearDatabaseCommandProvider
{
    private static string? _sqlCommand;

    public static async Task<string> GetAsync()
    {
        if (_sqlCommand is null)
        {
            _sqlCommand = await LoadCommandAsync();
        }

        return _sqlCommand;
    }

    private static Task<string> LoadCommandAsync()
    {
        var executingAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var commandPath = Path.Combine(executingAssemblyLocation, $"Clear", "ClearDatabase.sql");
        return File.ReadAllTextAsync(commandPath);
    }
}
