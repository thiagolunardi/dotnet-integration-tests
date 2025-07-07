using Microsoft.Extensions.Configuration;

namespace IntegrationTests.Tests;

public static class TestConfigurationAccessor
{
    static TestConfigurationAccessor()
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Test.json", optional: true)
            .Build();

        ConnectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public static string ConnectionString { get; }
}