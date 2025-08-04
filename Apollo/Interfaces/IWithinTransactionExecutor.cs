namespace Apollo.Core.Interfaces
{
    public interface IWithinTransactionExecutor
    {
        void ExecuteNonQuery(string statement, Dictionary<string, object>? parameters = null);
        List<T> ExecuteQuery<T>(string statement, Func<IRowParser, T> parseFunction, Dictionary<string, object>? parameters = null);
    }
}
