using System;
using System.Data.SqlClient;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class SqlClientUtilTests
    {
        public class CreateSqlErrorMethod
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                int infoNumber = 0;
                byte errorState = 0;
                byte errorClass = 0;
                string server = "server";
                string errorMessage = "errorMessage";
                string procedure = "procedure";
                int lineNumber = 0;
                uint win32ErrorCode = 0;

                // Act
                var error = SqlClientUtil.CreateSqlError(infoNumber, errorState, errorClass, server, errorMessage, procedure, lineNumber, win32ErrorCode);

                // Assert
                Assert.IsAssignableFrom<SqlError>(error);
            }
        }

        public class CreateSqlErrorCollectionMethod
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var errorCollection = SqlClientUtil.CreateSqlErrorCollection();

                // Assert
                Assert.IsAssignableFrom<SqlErrorCollection>(errorCollection);
            }
        }

        public class AddMethod
        {
            [Fact]
            public void AddsError()
            {
                // Arrange
                var errorCollection = SqlClientUtil.CreateSqlErrorCollection();
                int infoNumber = 0;
                byte errorState = 0;
                byte errorClass = 0;
                string server = "server";
                string errorMessage = "errorMessage";
                string procedure = "procedure";
                int lineNumber = 0;
                uint win32ErrorCode = 0;
                var error = SqlClientUtil.CreateSqlError(infoNumber, errorState, errorClass, server, errorMessage, procedure, lineNumber, win32ErrorCode);

                // Act
                errorCollection.Add(error);

                // Assert
                Assert.Same(error, errorCollection[0]);
            }
        }

        public class CreateSqlExceptionMethod
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                string message = null;
                SqlErrorCollection errorCollection = null;
                Exception innerException = null;
                Guid conId = default;

                // Act
                var sqlException = SqlClientUtil.CreateSqlException(message, errorCollection, innerException, conId);

                // Assert
                Assert.IsAssignableFrom<SqlException>(sqlException);
            }
        }
    }
}
