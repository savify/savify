using NUnit.Framework;

namespace App.BuildingBlocks.Tests.IntegrationTests;

public class EnvironmentVariablesProvider
{
    public static string? GetVariable(string variableName)
    {
        var environmentVariable = Environment.GetEnvironmentVariable(variableName);
        
        if (!string.IsNullOrEmpty(environmentVariable))
        {
            return environmentVariable;
        }

        environmentVariable = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User);

        if (!string.IsNullOrEmpty(environmentVariable))
        {
            return environmentVariable;
        }

        environmentVariable = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine);

        if (!string.IsNullOrEmpty(environmentVariable))
        {
            return environmentVariable;
        }

        return TestContext.Parameters.Get(variableName);
    }
}
