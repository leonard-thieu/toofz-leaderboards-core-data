using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class EntityFrameworkExtensionsTests
    {
        [Trait("Category", "Uses SQL Server")]
        [Collection(DatabaseCollection.Name)]
        public class GetMappingFragmentMethod
        {
            [Fact]
            public void ReturnsMappingFragment()
            {
                // Arrange
                using (var db = new LeaderboardsContext())
                {
                    // Act
                    var mappingFragment = db.GetMappingFragment<Entry>();

                    // Assert
                    Assert.IsAssignableFrom<MappingFragment>(mappingFragment);
                }
            }
        }

        [Trait("Category", "Uses SQL Server")]
        [Collection(DatabaseCollection.Name)]
        public class GetTableNameMethod
        {
            [Fact]
            public void ReturnsTableName()
            {
                // Arrange
                using (var db = new LeaderboardsContext())
                {
                    var mappingFragment = db.GetMappingFragment<Entry>();

                    // Act
                    var tableName = mappingFragment.GetTableName();

                    // Assert
                    Assert.Equal("Entries", tableName);
                }
            }
        }

        [Trait("Category", "Uses SQL Server")]
        [Collection(DatabaseCollection.Name)]
        public class GetScalarPropertyMappingsMethod
        {
            [Fact]
            public void ReturnsScalarPropertyMappings()
            {
                // Arrange
                using (var db = new LeaderboardsContext())
                {
                    var mappingFragment = db.GetMappingFragment<Entry>();

                    // Act
                    var mappings = mappingFragment.GetScalarPropertyMappings();

                    // Assert
                    Assert.IsAssignableFrom<IEnumerable<ScalarPropertyMapping>>(mappings);
                }
            }
        }

        [Trait("Category", "Uses SQL Server")]
        [Collection(DatabaseCollection.Name)]
        public class GetColumnNamesMethod
        {
            [Fact]
            public void ReturnsColumnNames()
            {
                // Arrange
                using (var db = new LeaderboardsContext())
                {
                    var mappingFragment = db.GetMappingFragment<Entry>();

                    // Act
                    var columnNames = mappingFragment.GetColumnNames();

                    // Assert
                    var expected = new[]
                    {
                        nameof(Entry.LeaderboardId),
                        nameof(Entry.Rank),
                        nameof(Entry.SteamId),
                        nameof(Entry.ReplayId),
                        nameof(Entry.Score),
                        nameof(Entry.Zone),
                        nameof(Entry.Level),
                    };
                    Assert.Equal(expected, columnNames);
                }
            }
        }

        [Trait("Category", "Uses SQL Server")]
        [Collection(DatabaseCollection.Name)]
        public class GetPrimaryKeyColumnNamesMethod
        {
            [Fact]
            public void ReturnsPrimaryKeyColumnNames()
            {
                // Arrange
                using (var db = new LeaderboardsContext())
                {
                    var mappingFragment = db.GetMappingFragment<Entry>();

                    // Act
                    var primaryKeyColumnNames = mappingFragment.GetPrimaryKeyColumnNames();

                    // Assert
                    var expected = new[]
                    {
                        nameof(Entry.LeaderboardId),
                        nameof(Entry.Rank),
                    };
                    Assert.Equal(expected, primaryKeyColumnNames);
                }
            }
        }
    }
}
