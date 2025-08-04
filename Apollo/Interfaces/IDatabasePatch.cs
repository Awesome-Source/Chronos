using Apollo.Core.DataObjects;

namespace Apollo.Core.Interfaces
{
    public interface IDatabasePatch
    {
        PatchMetaInfo PatchMetaInfo { get; }
        void Execute(IWithinTransactionExecutor withinTransactionExecutor);
    }
}
