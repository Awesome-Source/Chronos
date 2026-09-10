using Apollo.Core.DataObjects;

namespace Apollo.Core.Interfaces
{
    public interface IDatabasePatch
    {
        PatchMetaInfo PatchMetaInfo { get; }

        void AfterExecution(IDatabaseAccessor databaseAccessor);
        void Execute(IWithinTransactionExecutor withinTransactionExecutor);
        void BeforeExecution(IDatabaseAccessor databaseAccessor);
        
    }
}
