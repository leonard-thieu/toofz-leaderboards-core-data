using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace toofz.NecroDancer.Leaderboards
{
    /// <summary>
    /// Wraps an instance of <see cref="SqlCommand"/> to provide more detailed error information.
    /// </summary>
    internal sealed class SqlCommandAdapter : IDisposable
    {
        /// <summary>
        /// Creates and returns an instance of <see cref="SqlCommandAdapter"/> that wraps a <see cref="SqlCommand"/> that is
        /// associated with the <see cref="SqlConnection"/>.
        /// </summary>
        /// <param name="connection">The <see cref="SqlConnection"/> to create the command for.</param>
        /// <returns>An instance of <see cref="SqlCommandAdapter"/> that wraps a <see cref="SqlCommand"/>.</returns>
        public static SqlCommandAdapter FromConnection(SqlConnection connection)
        {
            return new SqlCommandAdapter(connection.CreateCommand());
        }

        internal SqlCommandAdapter(SqlCommand command)
        {
            this.command = command;
        }

        private readonly SqlCommand command;

        /// <summary>
        /// Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source.
        /// </summary>
        public string CommandText
        {
            get => command.CommandText;
            set => command.CommandText = value;
        }

        /// <summary>
        /// Gets the <see cref="SqlParameterCollection"/>.
        /// </summary>
        public SqlParameterCollection Parameters
        {
            get => command.Parameters;
        }

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (SqlException ex)
            {
                throw new SqlCommandException(ex.Message, ex, CommandText);
            }
        }

        #region IDisposable Members

        private bool disposed;

        /// <summary>
        /// Releases all resources used by the <see cref="SqlCommandAdapter"/>.
        /// </summary>
        public void Dispose()
        {
            if (disposed) { return; }

            command.Dispose();

            disposed = true;
        }

        #endregion IDisposable Members
    }
}