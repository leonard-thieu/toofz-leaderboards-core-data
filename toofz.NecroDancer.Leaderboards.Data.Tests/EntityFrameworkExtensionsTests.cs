using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class EntityFrameworkExtensionsTests
    {
        [Trait("Category", "Uses SQL Server")]
        public class GetMappingFragmentMethod : DatabaseTestsBase
        {
            [Fact]
            public void ReturnsMappingFragment()
            {
                // Arrange -> Act
                var mappingFragment = db.GetMappingFragment<Entry>();

                // Assert
                Assert.IsAssignableFrom<MappingFragment>(mappingFragment);
            }
        }

        [Trait("Category", "Uses SQL Server")]
        public class GetTableNameMethod : DatabaseTestsBase
        {
            [Fact]
            public void ReturnsTableName()
            {
                // Arrange
                var mappingFragment = db.GetMappingFragment<Entry>();

                // Act
                var tableName = mappingFragment.GetTableName();

                // Assert
                Assert.Equal("Entries", tableName);
            }
        }

        [Trait("Category", "Uses SQL Server")]
        public class GetScalarPropertyMappingsMethod : DatabaseTestsBase
        {
            [Fact]
            public void ReturnsScalarPropertyMappings()
            {
                // Arrange
                var mappingFragment = db.GetMappingFragment<Entry>();

                // Act
                var mappings = mappingFragment.GetScalarPropertyMappings();

                // Assert
                Assert.IsAssignableFrom<IEnumerable<ScalarPropertyMapping>>(mappings);
            }
        }

        [Trait("Category", "Uses SQL Server")]
        public class GetColumnNamesMethod : DatabaseTestsBase
        {
            [Fact]
            public void ReturnsColumnNames()
            {
                // Arrange
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

        [Trait("Category", "Uses SQL Server")]
        public class GetPrimaryKeyColumnNamesMethod : DatabaseTestsBase
        {
            [Fact]
            public void ReturnsPrimaryKeyColumnNames()
            {
                // Arrange
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
