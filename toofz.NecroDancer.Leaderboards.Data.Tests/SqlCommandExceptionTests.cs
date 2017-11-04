﻿using System;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class SqlCommandExceptionTests
    {
        public class Constructor
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                string message = null;
                Exception inner = null;
                string commandText = null;

                // Act
                var ex = new SqlCommandException(message, inner, commandText);

                // Assert
                Assert.IsAssignableFrom<SqlCommandException>(ex);
            }

            [Fact]
            public void SetsCommandText()
            {
                // Arrange
                string message = null;
                Exception inner = null;
                string commandText = "myCommandText";

                // Act
                var ex = new SqlCommandException(message, inner, commandText);

                // Assert
                Assert.Equal(commandText, ex.CommandText);
            }
        }

        public new class ToString
        {
            [Fact]
            public void CommandTextIsNull_ReturnsSqlCommandExceptionAsString()
            {
                // Arrange
                string message = null;
                Exception inner = null;
                string commandText = null;

                // Act
                var ex = new SqlCommandException(message, inner, commandText);

                // Assert
                Assert.Equal("toofz.NecroDancer.Leaderboards.SqlCommandException: Exception of type 'toofz.NecroDancer.Leaderboards.SqlCommandException' was thrown.", ex.ToString());
            }

            [Fact]
            public void ReturnsSqlCommandExceptionAsString()
            {
                // Arrange
                string message = null;
                Exception inner = null;
                string commandText = "myCommandText";

                // Act
                var ex = new SqlCommandException(message, inner, commandText);

                // Assert
                Assert.Equal(@"toofz.NecroDancer.Leaderboards.SqlCommandException: Exception of type 'toofz.NecroDancer.Leaderboards.SqlCommandException' was thrown.

myCommandText", ex.ToString(), ignoreLineEndingDifferences: true);
            }
        }
    }
}