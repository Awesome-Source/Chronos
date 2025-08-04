using Apollo.Core.DataObjects;

namespace Apollo.Core.Interfaces
{
    public interface IPatchInfoRepository
    {
        List<PatchInstallationInfo> GetOrderedPatchInstallationInfos(IWithinTransactionExecutor withinTransactionExecutor);
        void RegisterPatchInstallation(IWithinTransactionExecutor withinTransactionExecutor, PatchMetaInfo patchInfo);
    }
}
