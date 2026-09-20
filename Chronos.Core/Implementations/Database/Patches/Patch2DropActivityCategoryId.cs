using Apollo.Core.DataObjects;
using Apollo.Core.Interfaces;
using Apollo.Sqlite;

namespace Chronos.Core.Implementations.Database.Patches
{
    /// <summary>
    /// Patch1Categories added a NOT NULL activities.category_id column, but Activity has no
    /// category concept (only Objective does) and nothing ever populates it, so every
    /// INSERT INTO activities has failed its NOT NULL constraint since that patch. Drops
    /// the unused column.
    /// </summary>
    internal class Patch2DropActivityCategoryId : IDatabasePatch
    {
        public PatchMetaInfo PatchMetaInfo => new PatchMetaInfo(2, "Removed unused activities.category_id column");

        public void BeforeExecution(IConnectionExecutor connectionExecutor)
        {
            connectionExecutor.ExecuteNonQuery("PRAGMA FOREIGN_KEYS = OFF");
        }

        public void Execute(IWithinTransactionExecutor withinTransactionExecutor)
        {
            // SQLite refuses a plain DROP COLUMN here because category_id is part of a
            // FOREIGN KEY definition on this table, so the column is dropped by recreating
            // the table instead (same approach Patch1Categories used to add it).
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE patch_activities (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            name TEXT NOT NULL,
                                            time_account_id INTEGER NOT NULL,
                                            FOREIGN KEY (time_account_id) REFERENCES time_accounts (id)
                                        ) STRICT;");

            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO patch_activities (id, name, time_account_id) SELECT a.id, a.name, a.time_account_id FROM activities a");

            withinTransactionExecutor.ExecuteNonQuery("DROP TABLE activities");
            withinTransactionExecutor.ExecuteNonQuery("ALTER TABLE patch_activities RENAME TO activities");

            ForeignKeyChecker.Execute(withinTransactionExecutor);
        }

        public void AfterExecution(IConnectionExecutor connectionExecutor)
        {
            connectionExecutor.ExecuteNonQuery("PRAGMA FOREIGN_KEYS = ON");
        }
    }
}
