using Apollo.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace Apollo.Sqlite
{
    public class SqliteInMemoryDataBaseAccessor : IDatabaseAccessor
    {
        private bool _alreadyDisposed = false;
        private readonly SqliteConnection _connection;

        public SqliteInMemoryDataBaseAccessor()
        {
            //_connection = new SqliteConnection("file::memory:?cache=shared;Foreign Keys=True");
            _connection = new SqliteConnection("Data Source=Sharable;Mode=Memory;Cache=Shared;Foreign Keys=True");
            _connection.Open();            
        }

        public void ExecuteOnSingleConnection(Action<IConnectionExecutor> action)
        {
            lock (_connection)
            {
                action(new SqliteConnectionExecutor(_connection));
            }
        }

        public void ExecuteInTransaction(Action<IWithinTransactionExecutor> queryAction)
        {
            lock (_connection)
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
        }

        public T ExecuteInTransaction<T>(Func<IWithinTransactionExecutor, T> queryFunction)
        {
            lock (_connection)
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
        }

        public void ExecuteNonQuery(string statement, Dictionary<string, object>? parameters = null)
        {
            lock(_connection)
            {
                var withinTransactionExecutor = new SqliteWithinTransactionExecutor(_connection, null);
                withinTransactionExecutor.ExecuteNonQuery(statement, parameters);
            }            
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

            lock (_connection)
            {

                if (disposing)
                {
                    _connection.Close();
                    _connection.Dispose();
                }

                _alreadyDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
