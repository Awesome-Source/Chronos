namespace Apollo.Core.DataObjects
{
    public class PatchInstallationInfo
    {
        public int PatchNumber { get; set; }
        public string Comment { get; set; }
        public DateTime InstallationDate { get; set; }

        public PatchInstallationInfo(int patchNumber, string comment, DateTime installationDate)
        {
            PatchNumber = patchNumber;
            Comment = comment;
            InstallationDate = installationDate;
        }

        public override string ToString()
        {
            return $"Patch {PatchNumber} ({InstallationDate}): {Comment}";
        }
    }
}
