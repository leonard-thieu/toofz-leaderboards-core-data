using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using toofz.NecroDancer.Leaderboards.Migrations;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class MigrationsTests
    {
        #region From https://stackoverflow.com/a/42643788/414137

        [Trait("Category", "Uses SQL Server")]
        public class IntegrationTests : DatabaseTestsBase
        {
            [Fact]
            public void MigrationsUpDownTest()
            {
                var configuration = new Configuration();
                var migrator = new DbMigrator(configuration);

                // Retrieve migrations
                var migrations = new List<string> { "0" };  // Not sure if "0" is more zero than the first item in list of local migrations
                migrations.AddRange(migrator.GetLocalMigrations());

                try
                {
                    migrator.Update(migrations.First());

                    for (int index = 0; index < migrations.Count; index++)
                    {
                        migrator.Update(migrations[index]);
                        if (index > 0)
                            migrator.Update(migrations[index - 1]);
                    }

                    migrator.Update(migrations.Last());
                }
                catch (SqlException ex)
                {
                    Assert.True(false, "Should not have any errors when running migrations up and down: " + ex.Errors[0].Message.ToString());
                }
            }

            [Fact]
            public void PendingModelChangesTest()
            {
                // NOTE: Using MigratorScriptingDecorator so changes won't be made to the database
                var targetDatabase = new DbConnectionInfo(nameof(LeaderboardsContext));
                var migrationsConfiguration = new Configuration { TargetDatabase = targetDatabase };
                var migrator = new DbMigrator(migrationsConfiguration);
                var scriptingMigrator = new MigratorScriptingDecorator(migrator);

                try
                {
                    // NOTE: Using InitialDatabase so history won't be read from the database
                    scriptingMigrator.ScriptUpdate(DbMigrator.InitialDatabase, null);
                }
                catch (AutomaticMigrationsDisabledException)
                {
                    Assert.True(false, "Should be no pending model changes/migrations should cover all model changes.");
                }
            }
        }

        #endregion
    }
}
