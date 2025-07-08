using DbUp;
using IntegrationTests.Database;

namespace IntegrationTests.Tests;

// ReSharper disable once ClassNeverInstantiated.Global
public class TestDatabaseCollectionFixture
{
    public TestDatabaseCollectionFixture()
    {
        var connectionString = TestConfigurationAccessor.ConnectionString;
        
        DropDatabase.For.SqlDatabase(connectionString);
        EnsureDatabase.For.SqlDatabase(connectionString);
        
        var upgradeEngine = DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(DatabaseMigration).Assembly)
            .LogToConsole()
            .Build();

        var upgradeResult = upgradeEngine.PerformUpgrade();
        if (!upgradeResult.Successful)
        {
            throw upgradeResult.Error;
        }
    }
}