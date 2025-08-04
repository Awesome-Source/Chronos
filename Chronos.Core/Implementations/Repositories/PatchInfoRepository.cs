using Apollo.Core.DataObjects;
using Apollo.Core.Interfaces;

namespace Chronos.Core.Implementations.Repositories
{
    internal class PatchInfoRepository : IPatchInfoRepository
    {
        public List<PatchInstallationInfo> GetOrderedPatchInstallationInfos(IWithinTransactionExecutor withinTransactionExecutor)
        {
            withinTransactionExecutor.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS patch_installations (
                                                        	patch_number INT NOT NULL PRIMARY KEY,
                                                        	installation_date TEXT NOT NULL,
                                                        	patch_comment TEXT NOT NULL
                                                        );");

            var statement = "SELECT patch_number, patch_comment, installation_date FROM patch_installations;";
            var installedPatches = withinTransactionExecutor.ExecuteQuery(statement, (rp) => new PatchInstallationInfo(
            
                rp.GetInt("patch_number"),
                rp.GetString("patch_comment"),
                rp.GetDateTime("installation_date")
            ));

            return installedPatches
                .OrderBy(p => p.PatchNumber)
                .ToList();
        }

        public void RegisterPatchInstallation(IWithinTransactionExecutor withinTransactionExecutor, PatchMetaInfo patchInfo)
        {
            var parameters = new Dictionary<string, object>()
            {
                {"PATCH_NUMBER", patchInfo.PatchNumber },
                {"PATCH_COMMENT", patchInfo.Comment },
                {"INSTDATE", DateTime.Now }
            };

            withinTransactionExecutor.ExecuteNonQuery("INSERT INTO patch_installations (patch_number, patch_comment, installation_date) VALUES (@PATCH_NUMBER, @PATCH_COMMENT, @INSTDATE);", parameters);
        }
    }
}
