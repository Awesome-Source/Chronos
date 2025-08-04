using Apollo.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace Apollo.Sqlite
{
    public class SqliteWithinTransactionExecutor : IWithinTransactionExecutor
    {
        private readonly SqliteTransaction _transaction;
        private readonly SqliteConnection _connection;

        public SqliteWithinTransactionExecutor(SqliteConnection connection, SqliteTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public void ExecuteNonQuery(string statement, Dictionary<string, object>? parameters = null)
        {
            var sqlCommand = CreateCommandWithParameters(statement, parameters);
            sqlCommand.ExecuteNonQuery();
        }

        public List<T> ExecuteQuery<T>(string statement, Func<IRowParser, T> parseFunction, Dictionary<string, object>? parameters = null)
        {
            var result = new List<T>();
            var columnLookup = new Dictionary<string, int>();
            var sqlCommand = CreateCommandWithParameters(statement, parameters);

            using (var reader = sqlCommand.ExecuteReader())
            {
                FillColumnLookup(columnLookup, reader);
                var parser = new SqliteRowParser(columnLookup, reader);
                AddParsedRows(parseFunction, result, parser, reader);
            }

            return result;
        }

        private SqliteCommand CreateCommandWithParameters(string statement, Dictionary<string, object>? parameters)
        {
            var sqlCommand = _connection.CreateCommand();
            sqlCommand.Transaction = _transaction;
            sqlCommand.CommandText = statement;
            AddParametersToCommand(sqlCommand, parameters);
            return sqlCommand;
        }

        private static void AddParsedRows<T>(Func<IRowParser, T> parseFunction, List<T> result, SqliteRowParser parser, SqliteDataReader reader)
        {
            while (reader.Read())
            {
                result.Add(parseFunction(parser));
            }
        }

        private static void FillColumnLookup(Dictionary<string, int> columnLookup, SqliteDataReader reader)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                columnLookup.Add(reader.GetName(i), i);
            }
        }

        private static void AddParametersToCommand(SqliteCommand sqlCommand, Dictionary<string, object>? parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var (name, value) in parameters)
            {
                sqlCommand.Parameters.AddWithValue(name, value);
            }

            sqlCommand.Prepare();
        }
    }
}
