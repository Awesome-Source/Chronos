using Apollo.Core.DataObjects;
using Apollo.Core.Interfaces;

namespace Chronos.Core.Implementations.Database.Patches
{
    internal class Patch3ObjectiveIsDone : IDatabasePatch
    {
        public PatchMetaInfo PatchMetaInfo => new PatchMetaInfo(3, "Added objectives.is_done column");

        public void BeforeExecution(IConnectionExecutor connectionExecutor)
        {
        }

        public void Execute(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery("ALTER TABLE objectives ADD COLUMN is_done INTEGER NOT NULL DEFAULT 0");
        }

        public void AfterExecution(IConnectionExecutor connectionExecutor)
        {
        }
    }
}
