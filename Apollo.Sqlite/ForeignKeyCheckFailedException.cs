namespace Apollo.Sqlite
{
    public class ForeignKeyCheckFailedException : Exception
    {
        public IReadOnlyList<ForeignKeyCheckResult> ForeignKeyCheckResults { get; }

        public ForeignKeyCheckFailedException(IReadOnlyList<ForeignKeyCheckResult> foreignKeyCheckResults) : base("At least one foreign key is violated.")
        {
            ForeignKeyCheckResults = foreignKeyCheckResults;
        }        
    }
}
