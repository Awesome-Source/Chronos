using Apollo.Core.Interfaces;
using Microsoft.Data.Sqlite;

namespace Chronos.Core.Implementations.Database
{
    internal class DatabaseConnectionConfiguration : IDatabaseConnectionConfiguration
    {
        private readonly string _databasePath;

        public DatabaseConnectionConfiguration(string appDataDirectory)
        {
            _databasePath = Path.Combine(appDataDirectory, "chronos.db");
        }

        public string ConnectionString => $"Data Source={_databasePath};Mode=ReadWriteCreate;Foreign Keys=True";
    }
}
