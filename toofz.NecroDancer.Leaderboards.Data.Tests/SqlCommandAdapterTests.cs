using System.Data.SqlClient;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class SqlCommandAdapterTests
    {
        public class CommandTextProperty
        {
            [Fact]
            public void GetSetBehavior()
            {
                // Arrange
                var command = new SqlCommand();
                var adapter = new SqlCommandAdapter(command);

                // Act -> Assert
                adapter.CommandText = "myCommandText";
                Assert.Equal("myCommandText", adapter.CommandText);
            }
        }

        public class ParametersProperty
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                var command = new SqlCommand();
                var adapter = new SqlCommandAdapter(command);

                // Act
                var parameters = adapter.Parameters;

                // Assert
                Assert.IsAssignableFrom<SqlParameterCollection>(parameters);
            }
        }
    }
}
