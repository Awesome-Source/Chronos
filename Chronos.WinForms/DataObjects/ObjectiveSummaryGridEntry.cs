using System.ComponentModel;

namespace Chronos.WinForms.DataObjects
{
    internal class ObjectiveSummaryGridEntry
    {
        [DisplayName("Objective")]
        public string ObjectiveName { get; }
        public TimeSpan Duration { get; }

        public ObjectiveSummaryGridEntry(string objectiveName, TimeSpan duration)
        {
            ObjectiveName = objectiveName;
            Duration = duration;
        }        
    }
}
