using Apollo.Core.DataObjects;

namespace Apollo.Core.Interfaces
{
    public interface IDatabasePatch
    {
        PatchMetaInfo PatchMetaInfo { get; }

        void AfterExecution(IConnectionExecutor connectionExecutor);
        void Execute(IWithinTransactionExecutor withinTransactionExecutor);
        void BeforeExecution(IConnectionExecutor connectionExecutor);
        
    }
}
