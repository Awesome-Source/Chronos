using Apollo.Core.DataObjects;
using Apollo.Core.Interfaces;

namespace Chronos.Core.Implementations.Database.Patches
{
    internal class InitialPatch : IDatabasePatch
    {
        public PatchMetaInfo PatchMetaInfo => new PatchMetaInfo(0, "Initial");

        public void Execute(IWithinTransactionExecutor withinTransactionExecutor)
        {
            CreateTimeAccountsTable(withinTransactionExecutor);
            CreateActivitiesTable(withinTransactionExecutor);
            CreateObjectivesTable(withinTransactionExecutor);
            CreateTrackingDaysTable(withinTransactionExecutor);
            CreateTrackingTargetsTable(withinTransactionExecutor);
            CreateTrackingRecordsTable(withinTransactionExecutor);
            CreateActiveTrackingRecordTable(withinTransactionExecutor);
        }

        private void CreateTimeAccountsTable(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE time_accounts (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            name TEXT NOT NULL,
                                            color TEXT NOT NULL,
                                            is_worktime INTEGER NOT NULL
                                        ) STRICT;");
        }

        private void CreateActivitiesTable(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE activities (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            name TEXT NOT NULL,
                                            time_account_id INTEGER NOT NULL,
                                            FOREIGN KEY (time_account_id) REFERENCES time_accounts (id) 
                                        ) STRICT;");
        }

        private void CreateObjectivesTable(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE objectives (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            name TEXT NOT NULL,
                                            description TEXT NOT NULL
                                        ) STRICT;");
        }

        private void CreateTrackingDaysTable(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE tracking_days (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            year INTEGER NOT NULL,
                                            month INTEGER NOT NULL,
                                            day INTEGER NOT NULL,
                                            UNIQUE(year, month, day)
                                        ) STRICT;");
        }

        private void CreateTrackingTargetsTable(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE tracking_targets (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            tracking_day_id INTEGER NOT NULL,
                                            activity_id INTEGER NOT NULL,
                                            objective_id INTEGER NOT NULL,
                                            is_planned INTEGER NOT NULL,
                                            FOREIGN KEY (tracking_day_id) REFERENCES tracking_days (id),
                                            FOREIGN KEY (activity_id) REFERENCES activities (id),
                                            FOREIGN KEY (objective_id) REFERENCES objectives (id)
                                        ) STRICT;");
        }

        private void CreateTrackingRecordsTable(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE tracking_records (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            tracking_target_id INTEGER NOT NULL,
                                            start_time INTEGER NOT NULL,
                                            end_time INTEGER NOT NULL,
                                            duration INTEGER NOT NULL,
                                            FOREIGN KEY (tracking_target_id) REFERENCES tracking_targets (id)
                                        ) STRICT;");
        }

        private void CreateActiveTrackingRecordTable(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE active_tracking_record (
                                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            tracking_target_id INTEGER NOT NULL,
                                            start_time INTEGER NOT NULL,
                                            FOREIGN KEY (tracking_target_id) REFERENCES tracking_targets (id)
                                        ) STRICT;");
        }
    }
}
