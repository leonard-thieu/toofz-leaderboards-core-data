using System.Data.SqlClient;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class SqlConnectionExtensionsTests
    {
        public class GetSwitchTableCommandMethod
        {
            [Fact]
            public void ReturnsSwitchTableCommand()
            {
                // Arrange
                var connection = new SqlConnection();
                var viewName = "myViewName";
                var tableName = "myTableName";
                var columns = new[]
                {
                    "key_part_1",
                    "key_part_2",
                    "nullable_text",
                    "nullable_number",
                    "nullable_datetimeoffset",
                    "nullable_money",
                    "nullable_varbinary",
                    "nullable_image",
                };

                // Act
                var command = SqlConnectionExtensions.GetSwitchTableCommand(connection, viewName, tableName, columns, null);

                // Assert
                Assert.Equal(@"ALTER VIEW [myViewName]
AS

SELECT [key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image]
FROM [myTableName];", command.CommandText, ignoreLineEndingDifferences: true);
            }
        }

        public class GetTruncateTableCommand
        {
            [Fact]
            public void ReturnsTruncateTableCommand()
            {
                // Arrange
                var connection = new SqlConnection();
                var tableName = "myTableName";

                // Act
                var command = SqlConnectionExtensions.GetTruncateTableCommand(connection, tableName, null);

                // Assert
                Assert.Equal("TRUNCATE TABLE [myTableName];", command.CommandText);
            }
        }

        public class GetSelectIntoTemporaryTableCommandMethod
        {
            [Fact]
            public void ReturnsSelectIntoTemporaryTableCommand()
            {
                // Arrange
                var connection = new SqlConnection();
                var baseTableName = "myTableName";
                var tempTableName = "#myTableName";

                // Act
                var comnand = SqlConnectionExtensions.GetSelectIntoTemporaryTableCommand(connection, baseTableName, tempTableName, null);

                // Assert
                Assert.Equal("SELECT TOP 0 * INTO [#myTableName] FROM [myTableName];", comnand.CommandText);
            }
        }

        public class GetMergeCommandMethod
        {
            [Fact]
            public void UpdateWhenMatchedIsTrue_ReturnsMergeCommandWithUpdateWhenMatched()
            {
                // Arrange
                var connection = new SqlConnection();
                var targetTable = "myTableName";
                var tableSource = "myTableSource";
                var columns = new[]
                {
                    "key_part_1",
                    "key_part_2",
                    "nullable_text",
                    "nullable_number",
                    "nullable_datetimeoffset",
                    "nullable_money",
                    "nullable_varbinary",
                    "nullable_image",
                };
                var primaryKeyColumns = new[]
                {
                    "key_part_1",
                    "key_part_2",
                };
                var updateWhenMatched = true;

                // Act
                var command = SqlConnectionExtensions.GetMergeCommand(connection, targetTable, tableSource, columns, primaryKeyColumns, updateWhenMatched, null);

                // Assert
                Assert.Equal(@"MERGE INTO [myTableName] AS [Target]
USING [myTableSource] AS [Source]
    ON ([Target].[key_part_1] = [Source].[key_part_1] AND [Target].[key_part_2] = [Source].[key_part_2])
WHEN MATCHED
    THEN
        UPDATE
        SET [Target].[nullable_text] = [Source].[nullable_text],
            [Target].[nullable_number] = [Source].[nullable_number],
            [Target].[nullable_datetimeoffset] = [Source].[nullable_datetimeoffset],
            [Target].[nullable_money] = [Source].[nullable_money],
            [Target].[nullable_varbinary] = [Source].[nullable_varbinary],
            [Target].[nullable_image] = [Source].[nullable_image]
WHEN NOT MATCHED
    THEN
        INSERT ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image])
        VALUES ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image]);
", command.CommandText, ignoreLineEndingDifferences: true);
            }

            [Fact]
            public void UpdateWhenMatchedIsFalse_ReturnsMergeCommand()
            {
                // Arrange
                var connection = new SqlConnection();
                var targetTable = "myTableName";
                var tableSource = "myTableSource";
                var columns = new[]
                {
                    "key_part_1",
                    "key_part_2",
                    "nullable_text",
                    "nullable_number",
                    "nullable_datetimeoffset",
                    "nullable_money",
                    "nullable_varbinary",
                    "nullable_image",
                };
                var primaryKeyColumns = new[]
                {
                    "key_part_1",
                    "key_part_2",
                };
                var updateWhenMatched = false;

                // Act
                var command = SqlConnectionExtensions.GetMergeCommand(connection, targetTable, tableSource, columns, primaryKeyColumns, updateWhenMatched, null);

                // Assert
                Assert.Equal(@"MERGE INTO [myTableName] AS [Target]
USING [myTableSource] AS [Source]
    ON ([Target].[key_part_1] = [Source].[key_part_1] AND [Target].[key_part_2] = [Source].[key_part_2])
WHEN NOT MATCHED
    THEN
        INSERT ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image])
        VALUES ([key_part_1], [key_part_2], [nullable_text], [nullable_number], [nullable_datetimeoffset], [nullable_money], [nullable_varbinary], [nullable_image]);
", command.CommandText, ignoreLineEndingDifferences: true);
            }
        }

        public class GetAlterNonclusteredIndexesCommand
        {
            [Fact]
            public void ReturnsAlterNonclusteredIndexesCommand()
            {
                // Arrange
                var connection = new SqlConnection();
                var tableName = "myTableName";
                var action = "DISABLE";

                // Act
                var command = SqlConnectionExtensions.GetAlterNonclusteredIndexesCommand(connection, tableName, action, null);

                // Assert
                Assert.Equal(@"DECLARE @sql AS VARCHAR(MAX)='';

SELECT @sql = @sql + 'ALTER INDEX ' + sys.indexes.name + ' ON ' + sys.objects.name + ' DISABLE;' + CHAR(13) + CHAR(10)
FROM sys.indexes
JOIN sys.objects ON sys.indexes.object_id = sys.objects.object_id
WHERE sys.indexes.type_desc = 'NONCLUSTERED'
  AND sys.objects.type_desc = 'USER_TABLE'
  AND sys.objects.name = @tableName;

EXEC(@sql);", command.CommandText, ignoreLineEndingDifferences: true);
                Assert.True(command.Parameters.Contains("@tableName"));
            }
        }
    }
}
