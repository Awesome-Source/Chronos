using Apollo.Core.Interfaces;

namespace Apollo.Sqlite
{
    public static class ForeignKeyChecker
    {
        public static void Execute(IWithinTransactionExecutor withinTransactionExecutor)
        {
            var foreignKeyCheckResults = withinTransactionExecutor.ExecuteQuery("PRAGMA foreign_key_check", ForeignKeyCheckResult.ParseFromPragma);
            if (foreignKeyCheckResults.Any())
            {
                throw new ForeignKeyCheckFailedException(foreignKeyCheckResults);
            }
        }
    }
}
