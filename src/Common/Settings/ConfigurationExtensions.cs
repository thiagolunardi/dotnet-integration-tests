using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.Common.Settings;

public static class ConfigurationExtensions
{
    public static void AddAppSettings(this IConfigurationBuilder configuration)
    {
        var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        configuration
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddJsonFile("appsettings.Test.json", optional: true);
    }
}
