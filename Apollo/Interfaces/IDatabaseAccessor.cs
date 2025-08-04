namespace Apollo.Core.Interfaces
{
    public interface IDatabaseAccessor
    {
        void ExecuteNonQuery(string statement, Dictionary<string, object>? parameters = null);
        List<T> ExecuteQuery<T>(string statement, Func<IRowParser, T> parseFunction, Dictionary<string, object>? parameters = null);
        T ExecuteInTransaction<T>(Func<IWithinTransactionExecutor, T> queryFunction);
        void ExecuteInTransaction(Action<IWithinTransactionExecutor> queryAction);
    }
}
