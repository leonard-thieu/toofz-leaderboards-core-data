using System;
using System.Data.SqlClient;
using System.Text;

namespace toofz.NecroDancer.Leaderboards
{
    public sealed class SqlCommandException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommandException"/> class with a specified error message and
        /// a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic)
        /// if no inner exception is specified.
        /// </param>
        /// <param name="commandText">
        /// The Transact-SQL statement, table name or stored procedure that was attempted to be executed.
        /// </param>
        public SqlCommandException(string message, SqlException innerException, string commandText) : base(message, innerException)
        {
            CommandText = commandText;
        }

        /// <summary>
        /// Gets or sets the Transact-SQL statement, table name or stored procedure that was attempted to be executed.
        /// </summary>
        public string CommandText { get; }

        /// <summary>
        /// Gets the <see cref="SqlException"/> instance that caused the current exception.
        /// </summary>
        /// <returns>
        /// An object that describes the error that caused the current exception. The <see cref="InnerException"/> 
        /// property returns the same value as was passed into the <see cref="SqlCommandException(string, SqlException, string)"/>
        /// constructor, or null if the inner exception value was not supplied to the constructor. 
        /// This property is read-only.
        /// </returns>
        public new SqlException InnerException => (SqlException)base.InnerException;

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(base.ToString());
            sb.AppendLine();
            sb.AppendLine(CommandText);

            return sb.ToString().Trim();
        }
    }
}