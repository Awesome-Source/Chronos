using Apollo.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace Apollo.Sqlite
{
    /// <summary>
    /// Every operation rents its own connection from the (shared) Microsoft.Data.Sqlite connection pool and
    /// returns it when done, so several transactions can run in parallel.
    /// </summary>
    public class SqliteDataBaseAccessor : IDatabaseAccessor
    {
        private readonly string _connectionString;

        public SqliteDataBaseAccessor(IDatabaseConnectionConfiguration configuration)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder(configuration.ConnectionString)
            {
                Pooling = true
            };
            _connectionString = connectionStringBuilder.ToString();

            EnableWriteAheadLogging();
        }

        public void ExecuteOnSingleConnection(Action<IConnectionExecutor> action)
        {
            using var connection = OpenConnection();
            action(new SqliteConnectionExecutor(connection));
        }

        public void ExecuteInTransaction(Action<IWithinTransactionExecutor> queryAction)
        {
            using var connection = OpenConnection();
            new SqliteConnectionExecutor(connection).ExecuteInTransaction(queryAction);
        }

        public T ExecuteInTransaction<T>(Func<IWithinTransactionExecutor, T> queryFunction)
        {
            using var connection = OpenConnection();
            return new SqliteConnectionExecutor(connection).ExecuteInTransaction(queryFunction);
        }

        public void ExecuteNonQuery(string statement, Dictionary<string, object>? parameters = null)
        {
            using var connection = OpenConnection();
            new SqliteConnectionExecutor(connection).ExecuteNonQuery(statement, parameters);
        }

        public List<T> ExecuteQuery<T>(string statement, Func<IRowParser, T> parseFunction, Dictionary<string, object>? parameters = null)
        {
            using var connection = OpenConnection();
            return new SqliteConnectionExecutor(connection).ExecuteQuery(statement, parseFunction, parameters);
        }

        private SqliteConnection OpenConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }

        // The journal mode is persisted in the database file. WAL allows readers to run alongside a writer.
        private void EnableWriteAheadLogging()
        {
            using var connection = OpenConnection();
            new SqliteConnectionExecutor(connection).ExecuteNonQuery("PRAGMA journal_mode = WAL");
        }
    }
}
