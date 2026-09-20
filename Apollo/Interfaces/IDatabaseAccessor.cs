namespace Apollo.Core.Interfaces
{
    public interface IDatabaseAccessor : IConnectionExecutor
    {
        /// <summary>
        /// Executes the action with one connection that is held for the whole duration of the action.
        /// Use this when connection scoped state (e.g. PRAGMAs) has to be shared with the statements that follow.
        /// </summary>
        void ExecuteOnSingleConnection(Action<IConnectionExecutor> action);
    }
}
