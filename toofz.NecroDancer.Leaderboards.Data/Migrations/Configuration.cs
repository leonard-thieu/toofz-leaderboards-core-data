using System.Data.Entity.Migrations;

namespace toofz.NecroDancer.Leaderboards.Migrations
{
    sealed class Configuration : DbMigrationsConfiguration<LeaderboardsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "toofz.NecroDancer.Leaderboards.Migrations.Configuration";
        }
    }
}
