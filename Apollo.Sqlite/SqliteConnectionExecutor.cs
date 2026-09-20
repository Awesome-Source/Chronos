using Apollo.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace Apollo.Sqlite
{
    /// <summary>
    /// Executes statements on a single, already opened connection. The connection is owned by the caller.
    /// </summary>
    public class SqliteConnectionExecutor : IConnectionExecutor
    {
        private readonly SqliteConnection _connection;

        public SqliteConnectionExecutor(SqliteConnection connection)
        {
            _connection = connection;
        }

        public void ExecuteInTransaction(Action<IWithinTransactionExecutor> queryAction)
        {
            // BEGIN IMMEDIATE: the write lock is acquired up front (waiting up to the busy timeout)
            // instead of failing with SQLITE_BUSY when a read transaction is upgraded to a write.
            using var transaction = _connection.BeginTransaction(deferred: false);

            try
            {
                var withinTransactionExecutor = new SqliteWithinTransactionExecutor(_connection, transaction);
                queryAction(withinTransactionExecutor);
                transaction.Commit();
            }
            catch (Exception)
            {
                //TODO log exception
                transaction.Rollback();
                throw;
            }
        }

        public T ExecuteInTransaction<T>(Func<IWithinTransactionExecutor, T> queryFunction)
        {
            using var transaction = _connection.BeginTransaction(deferred: false);
            try
            {
                var withinTransactionExecutor = new SqliteWithinTransactionExecutor(_connection, transaction);
                var returnValue = queryFunction(withinTransactionExecutor);
                transaction.Commit();

                return returnValue;
            }
            catch (Exception)
            {
                //TODO log exception
                transaction.Rollback();
                throw;
            }
        }

        public void ExecuteNonQuery(string statement, Dictionary<string, object>? parameters = null)
        {
            var withinTransactionExecutor = new SqliteWithinTransactionExecutor(_connection, null);
            withinTransactionExecutor.ExecuteNonQuery(statement, parameters);
        }

        public List<T> ExecuteQuery<T>(string statement, Func<IRowParser, T> parseFunction, Dictionary<string, object>? parameters = null)
        {
            // Deferred transaction: pure reads must not take the write lock so that they can run in parallel.
            using var transaction = _connection.BeginTransaction();
            try
            {
                var withinTransactionExecutor = new SqliteWithinTransactionExecutor(_connection, transaction);
                var result = withinTransactionExecutor.ExecuteQuery(statement, parseFunction, parameters);
                transaction.Commit();

                return result;
            }
            catch (Exception)
            {
                //TODO log exception
                transaction.Rollback();
                throw;
            }
        }
    }
}
