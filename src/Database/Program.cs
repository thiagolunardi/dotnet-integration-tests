using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;

Console.WriteLine("Database Migrations Console Project");

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddJsonFile("appsettings.Development.json", optional: true)
    .Build();
    
var connectionString = configuration.GetConnectionString("DefaultConnection");

EnsureDatabase.For.SqlDatabase(connectionString);

var upgradeEngine = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .WithExecutionTimeout(TimeSpan.FromSeconds(5))
    .LogToConsole()
    .Build();

if (!upgradeEngine.TryConnect(out var errorMessage))
{
    Console.WriteLine($"Failed to connect to the database: {errorMessage}");
    return -1;
}

var result = upgradeEngine.PerformUpgrade();

Console.WriteLine(result.Successful ? "" : result.Error.Message);

return result.Successful ? 0 : -1;