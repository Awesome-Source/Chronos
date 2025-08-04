using Apollo.Core.DataObjects;
using Apollo.Core.Exceptions;
using Apollo.Core.Interfaces;

namespace Apollo.Core
{
    public class DatabaseInitializer
    {
        private readonly IDatabaseAccessor _databaseAccessor;
        private readonly IPatchInfoRepository _patchInfoRepository;

        public DatabaseInitializer(IDatabaseAccessor databaseAccessor, IPatchInfoRepository patchInfoRepository)
        {
            _databaseAccessor = databaseAccessor;
            _patchInfoRepository = patchInfoRepository;
        }

        public void Run(List<IDatabasePatch> patches)
        {
            EnsureAllPatchesAreInOrderWithoutGap(patches);
            var installedPatches = RetrieveInstalledPatchesFromDatabase();

            if (installedPatches == null)
            {
                throw new DatabaseInitializationException("Could not retrieve installed patches from database. Application will shutdown.");
            }

            if (!ValidateInstalledPatches(installedPatches))
            {
                throw new DatabaseInitializationException("Installed patches are not okay - application will shutdown.");
            }

            if (installedPatches.Count > patches.Count)
            {
                throw new DatabaseInitializationException("Unknown patch found in database. The application may be outdated!");
            }

            if (!InstallAllMissingPatches(patches, installedPatches.ToDictionary(p => p.PatchNumber)))
            {
                throw new DatabaseInitializationException("A patch couldn't be installed. The application will shutdown!");
            }
        }

        private void EnsureAllPatchesAreInOrderWithoutGap(List<IDatabasePatch> patches)
        {
            var currentVersion = 0;
            foreach (var patch in patches)
            {
                var comment = patch.PatchMetaInfo.Comment;
                var patchVersion = patch.PatchMetaInfo.PatchNumber;

                if (patchVersion != currentVersion)
                {
                    throw new DatabaseInitializationException($"The patch {comment} with version {patchVersion} has an unexpected version. Check for gaps, double versions or a wrong order of patches.");
                }

                currentVersion++;
            }
        }

        private bool ValidateInstalledPatches(List<PatchInstallationInfo> installedPatches)
        {
            for (var i = 0; i < installedPatches.Count; i++)
            {
                var patch = installedPatches[i];
                Console.WriteLine($"Found patch [{patch}]");

                if (patch.PatchNumber != i)
                {
                    Console.WriteLine($"Expected patch number {i}. Either the patches didn't start at zero or there is a version hole.");
                    return false;
                }
            }

            return true;
        }

        private List<PatchInstallationInfo>? RetrieveInstalledPatchesFromDatabase()
        {
            try
            {
                return _databaseAccessor.ExecuteInTransaction(_patchInfoRepository.GetOrderedPatchInstallationInfos);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); //TODO detailed logging
            }

            return null;
        }

        private bool InstallAllMissingPatches(List<IDatabasePatch> patches, Dictionary<int, PatchInstallationInfo> installedPatchByVersion)
        {
            foreach (var patch in patches)
            {
                var currentPatchNumber = patch.PatchMetaInfo.PatchNumber;
                if (installedPatchByVersion.ContainsKey(currentPatchNumber))
                {
                    Console.WriteLine($"Patch {currentPatchNumber} is already installed.");
                    continue;
                }

                var success = InstallPatch(patch);
                if (!success)
                {
                    Console.WriteLine($"Could not install patch {currentPatchNumber}.");
                    return false;
                }
            }

            return true;
        }

        private bool InstallPatch(IDatabasePatch patch)
        {
            try
            {
                Console.WriteLine($"Trying to install patch [{patch.GetType().FullName}]");
                _databaseAccessor.ExecuteInTransaction(withinTransactionExecutor =>
                {
                    patch.Execute(withinTransactionExecutor);
                    _patchInfoRepository.RegisterPatchInstallation(withinTransactionExecutor, patch.PatchMetaInfo);
                });

                Console.WriteLine($"Successfully installed patch {patch.PatchMetaInfo.PatchNumber}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); //TODO detailed logging
                return false;
            }

            return true;
        }
    }
}