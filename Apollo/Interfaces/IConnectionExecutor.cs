namespace Apollo.Core.Interfaces
{
    /// <summary>
    /// Database operations that are all executed on one and the same connection.
    /// </summary>
    public interface IConnectionExecutor
    {
        void ExecuteNonQuery(string statement, Dictionary<string, object>? parameters = null);
        List<T> ExecuteQuery<T>(string statement, Func<IRowParser, T> parseFunction, Dictionary<string, object>? parameters = null);
        T ExecuteInTransaction<T>(Func<IWithinTransactionExecutor, T> queryFunction);
        void ExecuteInTransaction(Action<IWithinTransactionExecutor> queryAction);
    }
}
