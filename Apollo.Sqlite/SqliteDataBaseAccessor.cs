using Apollo.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace Apollo.Sqlite
{
    public class SqliteDataBaseAccessor : IDatabaseAccessor, IDisposable
    {
        private bool _alreadyDisposed = false;
        private readonly SqliteConnection _connection;

        public SqliteDataBaseAccessor(IDatabaseConnectionConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString;
            _connection = new SqliteConnection(connectionString);
            _connection.Open();
        }

        public void ExecuteInTransaction(Action<IWithinTransactionExecutor> queryAction)
        {
            using var transaction = _connection.BeginTransaction();

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
            using var transaction = _connection.BeginTransaction();
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
            ExecuteInTransaction(withinTransactionExecutor => withinTransactionExecutor.ExecuteNonQuery(statement, parameters));
        }

        public List<T> ExecuteQuery<T>(string statement, Func<IRowParser, T> parseFunction, Dictionary<string, object>? parameters = null)
        {
            return ExecuteInTransaction(withinTransactionExecutor => withinTransactionExecutor.ExecuteQuery(statement, parseFunction, parameters));
        }

        private void Dispose(bool disposing)
        {
            if (_alreadyDisposed)
            {
                return;
            }

            if (disposing)
            {
                _connection.Close();
                _connection.Dispose();
            }

            _alreadyDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
