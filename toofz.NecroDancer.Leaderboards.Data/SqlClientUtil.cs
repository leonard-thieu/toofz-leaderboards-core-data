using System;
using System.Data.SqlClient;
using System.Reflection;

namespace toofz.NecroDancer.Leaderboards
{
    public static class SqlClientUtil
    {
        #region http://blog.gauffin.org/2014/08/how-to-create-a-sqlexception/

        #region SqlError

        public static SqlError CreateSqlError(int infoNumber, byte errorState, byte errorClass, string server, string errorMessage, string procedure, int lineNumber, uint win32ErrorCode)
        {
            var sqlErrorCtor = GetConstructor<SqlError>(
                typeof(int),    // infoNumber
                typeof(byte),   // errorState
                typeof(byte),   // errorClass
                typeof(string), // server
                typeof(string), // errorMessage
                typeof(string), // procedure
                typeof(int),    // lineNumber
                typeof(uint));  // win32ErrorCode

            return (SqlError)sqlErrorCtor.Invoke(new object[] { infoNumber, errorState, errorClass, server, errorMessage, procedure, lineNumber, win32ErrorCode });
        }

        #endregion

        #region SqlErrorCollection

        public static SqlErrorCollection CreateSqlErrorCollection()
        {
            var sqlErrorCollectionCtor = GetConstructor<SqlErrorCollection>();

            return (SqlErrorCollection)sqlErrorCollectionCtor.Invoke(null);
        }

        public static void Add(this SqlErrorCollection errorCollection, SqlError error)
        {
            var sqlErrorCollectionAdd = typeof(SqlErrorCollection).GetMethod("Add", NonPublicInstanceMember);

            sqlErrorCollectionAdd.Invoke(errorCollection, new[] { error });
        }

        #endregion

        #region SqlException

        public static SqlException CreateSqlException(string message, SqlErrorCollection errorCollection, Exception innerException, Guid conId)
        {
            var sqlExceptionCtor = GetConstructor<SqlException>(
                typeof(string),             // message
                typeof(SqlErrorCollection), // errorCollection
                typeof(Exception),          // innerException
                typeof(Guid));              // conId

            return (SqlException)sqlExceptionCtor.Invoke(new object[] { message, errorCollection, innerException, conId });
        }

        #endregion

        #region Helper Methods

        private const BindingFlags NonPublicInstanceMember = BindingFlags.NonPublic | BindingFlags.Instance;

        private static ConstructorInfo GetConstructor<T>(params Type[] types)
        {
            return typeof(T).GetConstructor(
                bindingAttr: NonPublicInstanceMember,
                binder: null,
                types: types,
                modifiers: null);
        }

        #endregion

        #endregion
    }
}
