using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace toofz.NecroDancer.Leaderboards.Migrations
{
    [ExcludeFromCodeCoverage]
    sealed class Configuration : DbMigrationsConfiguration<LeaderboardsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Toofz.NecroDancer.Leaderboards.DataAccess.Migrations.Configuration";
        }

        protected override void Seed(LeaderboardsContext context)
        {
            base.Seed(context);

            var dailyLeaderboardHeaders = LeaderboardsResources.ReadDailyLeaderboardHeaders();
            var existing = (from l in context.DailyLeaderboards
                            select l.LeaderboardId)
                            .ToList();
            var newDailies = from h in dailyLeaderboardHeaders
                             where !existing.Contains(h.Id)
                             select h;
            var leaderboardCategories = LeaderboardsResources.ReadLeaderboardCategories();
            var products = leaderboardCategories["products"];

            foreach (var newDaily in newDailies)
            {
                var leaderboard = new DailyLeaderboard
                {
                    LeaderboardId = newDaily.Id,
                    Date = newDaily.Date,
                    IsProduction = newDaily.IsProduction,
                };
                leaderboard.ProductId = products[newDaily.Product].Id;
                context.DailyLeaderboards.Add(leaderboard);
            }

            context.SaveChanges();
        }
    }
}
