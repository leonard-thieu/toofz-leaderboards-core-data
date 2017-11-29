using System;
using System.Data.SqlClient;
using System.Linq;
using Xunit;

namespace toofz.NecroDancer.Leaderboards.Tests
{
    public class SqlClientUtilTests
    {
        public class CreateSqlErrorMethod_Int32
        {
            [Fact]
            public void ReturnsInstance()
            {
                // Arrange
                int infoNumber = 0;

                // Act
                var error = SqlClientUtil.CreateSqlError(infoNumber);

                // Assert
                Assert.IsAssignableFrom<SqlError>(error);
            }
        }

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
            public void NoParams_ReturnsInstance()
            {
                // Arrange -> Act
                var errorCollection = SqlClientUtil.CreateSqlErrorCollection();

                // Assert
                Assert.IsAssignableFrom<SqlErrorCollection>(errorCollection);
            }

            [Fact]
            public void HasParams_ReturnsInstanceWithErrors()
            {
                // Arrange
                var sqlError = SqlClientUtil.CreateSqlError(0);

                // Act
                var errorCollection = SqlClientUtil.CreateSqlErrorCollection(sqlError);

                // Assert
                Assert.Contains(sqlError, errorCollection.OfType<SqlError>());
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

        public class CreateSqlExceptionMethod_Params_Array_SqlError
        {
            [Fact]
            public void NoParams_ReturnsInstance()
            {
                // Arrange -> Act
                var ex = SqlClientUtil.CreateSqlException();

                // Assert
                Assert.IsAssignableFrom<SqlException>(ex);
            }

            [Fact]
            public void HasParams_ReturnsInstanceWithErrors()
            {
                // Arrange
                var sqlError = SqlClientUtil.CreateSqlError(0);

                // Act
                var ex = SqlClientUtil.CreateSqlException(sqlError);

                // Assert
                Assert.Contains(sqlError, ex.Errors.OfType<SqlError>());
            }
        }

        public class CreateSqlExceptionMethod_String_Exception_Guid_Params_Array_SqlError
        {
            private readonly string message = "myMessage";
            private readonly Exception innerException = new Exception();
            private readonly Guid conId = new Guid();

            [Fact]
            public void NoParams_ReturnsInstance()
            {
                // Arrange -> Act
                var ex = SqlClientUtil.CreateSqlException(message, innerException, conId);

                // Assert
                Assert.IsAssignableFrom<SqlException>(ex);
            }

            [Fact]
            public void HasParams_ReturnsInstanceWithErrors()
            {
                // Arrange
                var sqlError = SqlClientUtil.CreateSqlError(0);

                // Act
                var ex = SqlClientUtil.CreateSqlException(message, innerException, conId, sqlError);

                // Assert
                Assert.Contains(sqlError, ex.Errors.OfType<SqlError>());
            }
        }

        public class CreateSqlExceptionMethod
        {
            private readonly string message = "myMessage";
            private readonly SqlErrorCollection errorCollection = SqlClientUtil.CreateSqlErrorCollection();
            private readonly Exception innerException = new Exception();
            private readonly Guid conId = new Guid();

            [Fact]
            public void ReturnsInstance()
            {
                // Arrange -> Act
                var sqlException = SqlClientUtil.CreateSqlException(message, errorCollection, innerException, conId);

                // Assert
                Assert.IsAssignableFrom<SqlException>(sqlException);
            }
        }
    }
}
