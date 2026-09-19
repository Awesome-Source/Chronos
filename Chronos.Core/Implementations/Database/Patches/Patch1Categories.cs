using Apollo.Core.DataObjects;
using Apollo.Core.Interfaces;
using Apollo.Sqlite;

namespace Chronos.Core.Implementations.Database.Patches
{
    internal class Patch1Categories : IDatabasePatch
    {
        public PatchMetaInfo PatchMetaInfo => new PatchMetaInfo(1, "Added categories");

        public void BeforeExecution(IDatabaseAccessor databaseAccessor)
        {
            databaseAccessor.ExecuteNonQuery("PRAGMA FOREIGN_KEYS = OFF");
        }        

        public void Execute(IWithinTransactionExecutor withinTransactionExecutor)
        {
            CreateCategoriesTable(withinTransactionExecutor);
            ExtendExistingTablesWithCategories(withinTransactionExecutor);
        }

        public void AfterExecution(IDatabaseAccessor databaseAccessor)
        {
            databaseAccessor.ExecuteNonQuery("PRAGMA FOREIGN_KEYS = ON");
        }

        private void CreateCategoriesTable(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE categories (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            name TEXT NOT NULL
                                        ) STRICT;");

            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO categories (name) VALUES ('-')");
        }

        private void ExtendExistingTablesWithCategories(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE patch_activities (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            name TEXT NOT NULL,
                                            time_account_id INTEGER NOT NULL,
                                            category_id INTEGER NOT NULL,
                                            FOREIGN KEY (time_account_id) REFERENCES time_accounts (id),
                                            FOREIGN KEY (category_id) REFERENCES categories (id) 
                                        ) STRICT;");

            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE patch_objectives (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            name TEXT NOT NULL,
                                            description TEXT NOT NULL,
                                            category_id INTEGER NOT NULL,
                                            FOREIGN KEY (category_id) REFERENCES categories (id) 
                                        ) STRICT;");

            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO patch_activities (id, name, time_account_id, category_id) SELECT a.id, a.name, a.time_account_id, 1 FROM activities a");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO patch_objectives (id, name, description, category_id) SELECT o.id, o.name, o.description, 1 FROM objectives o");

            withinTransactionExecutor.ExecuteNonQuery("DROP TABLE activities");
            withinTransactionExecutor.ExecuteNonQuery("DROP TABLE objectives");

            withinTransactionExecutor.ExecuteNonQuery("ALTER TABLE patch_activities RENAME TO activities");
            withinTransactionExecutor.ExecuteNonQuery("ALTER TABLE patch_objectives RENAME TO objectives");

            ForeignKeyChecker.Execute(withinTransactionExecutor);
        }
    }
}
