using Apollo.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace Apollo.Sqlite
{
    public class SqliteRowParser : IRowParser
    {
        private readonly Dictionary<string, int> _columnLookup;
        private readonly SqliteDataReader _reader;

        public SqliteRowParser(Dictionary<string, int> columnLookup, SqliteDataReader reader)
        {
            _columnLookup = columnLookup;
            _reader = reader;
        }

        public bool GetBool(string columnName)
        {
            return _reader.GetBoolean(_columnLookup[columnName]);
        }

        public short GetShort(string columnName)
        {
            return _reader.GetInt16(_columnLookup[columnName]);
        }

        public short? GetNullableShort(string columnName)
        {
            var columnIndex = _columnLookup[columnName];
            if (_reader.IsDBNull(columnIndex))
            {
                return null;
            }

            return _reader.GetInt16(columnIndex);
        }

        public int GetInt(string columnName)
        {
            return _reader.GetInt32(_columnLookup[columnName]);
        }

        public int? GetNullableInt(string columnName)
        {
            var columnIndex = _columnLookup[columnName];
            if (_reader.IsDBNull(columnIndex))
            {
                return null;
            }

            return _reader.GetInt32(columnIndex);
        }

        public long GetLong(string columnName)
        {
            return _reader.GetInt64(_columnLookup[columnName]);
        }

        public string GetString(string columnName)
        {
            return _reader.GetString(_columnLookup[columnName]);
        }

        public string? GetNullableString(string columnName)
        {
            var columnIndex = _columnLookup[columnName];
            if (_reader.IsDBNull(columnIndex))
            {
                return null;
            }

            return _reader.GetString(columnIndex);
        }

        public DateTime GetDateTime(string columnName)
        {
            return _reader.GetDateTime(_columnLookup[columnName]);
        }
    }
}
