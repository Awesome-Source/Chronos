namespace Apollo.Core.DataObjects
{
    public class PatchMetaInfo
    {
        public int PatchNumber { get; set; }
        public string Comment { get; set; }

        public PatchMetaInfo(int patchNumber, string comment)
        {
            PatchNumber = patchNumber;
            Comment = comment;
        }

        public override string ToString()
        {
            return $"Patch {PatchNumber}: {Comment}";
        }
    }
}
