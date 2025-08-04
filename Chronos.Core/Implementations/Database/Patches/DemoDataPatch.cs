using Apollo.Core.DataObjects;
using Apollo.Core.Interfaces;

namespace Chronos.Core.Implementations.Database.Patches
{
    internal class DemoDataPatch : IDatabasePatch
    {
        public PatchMetaInfo PatchMetaInfo => new PatchMetaInfo(1, "Demo data");

        public void Execute(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO time_accounts (name, color, is_worktime) VALUES ('Break', '#808080', 0)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO time_accounts (name, color, is_worktime) VALUES ('STD Projects', '#00FF00', 1)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO time_accounts (name, color, is_worktime) VALUES ('Support', '#FF0000', 1)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO time_accounts (name, color, is_worktime) VALUES ('Project Overheads', '#008000', 1)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO time_accounts (name, color, is_worktime) VALUES ('General Overheads', '#8000ff', 1)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO time_accounts (name, color, is_worktime) VALUES ('Custom Projects', '#FFFF00', 1)");

            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO activities (name, time_account_id) VALUES ('Estimation', 2)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO activities (name, time_account_id) VALUES ('Specification', 2)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO activities (name, time_account_id) VALUES ('Design', 2)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO activities (name, time_account_id) VALUES ('Implementation', 2)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO activities (name, time_account_id) VALUES ('Test', 2)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO activities (name, time_account_id) VALUES ('Planning', 4)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO activities (name, time_account_id) VALUES ('PST', 3)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO activities (name, time_account_id) VALUES ('Analysis', 3)");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO activities (name, time_account_id) VALUES ('Delivery', 3)");

            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO objectives (name, description) VALUES ('None', '')");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO objectives (name, description) VALUES ('#1337', 'Support Ticket 1337')");
            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO objectives (name, description) VALUES ('XYZ-42', 'Story 42')");
        }
    }
}
