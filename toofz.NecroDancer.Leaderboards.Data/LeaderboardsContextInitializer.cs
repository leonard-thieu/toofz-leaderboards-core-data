using System.Collections.Generic;
using System.Data.Entity;
using Newtonsoft.Json;
using toofz.NecroDancer.Leaderboards.Properties;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class LeaderboardsContextInitializer : CreateDatabaseIfNotExists<LeaderboardsContext>
    {
        protected override void Seed(LeaderboardsContext context)
        {
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(Resources.Products);
            context.Products.AddRange(products);
            var modes = JsonConvert.DeserializeObject<IEnumerable<Mode>>(Resources.Modes);
            context.Modes.AddRange(modes);
            var runs = JsonConvert.DeserializeObject<IEnumerable<Run>>(Resources.Runs);
            context.Runs.AddRange(runs);
            var characters = JsonConvert.DeserializeObject<IEnumerable<Character>>(Resources.Characters);
            context.Characters.AddRange(characters);
            var leaderboards = JsonConvert.DeserializeObject<IEnumerable<Leaderboard>>(Resources.Leaderboards);
            context.Leaderboards.AddRange(leaderboards);
            var dailyLeaderboards = JsonConvert.DeserializeObject<IEnumerable<DailyLeaderboard>>(Resources.DailyLeaderboards);
            context.DailyLeaderboards.AddRange(dailyLeaderboards);

            context.SaveChanges();
        }
    }
}
