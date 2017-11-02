using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    [CollectionDefinition(Name)]
    public sealed class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        public const string Name = "Database collection";
    }
}
