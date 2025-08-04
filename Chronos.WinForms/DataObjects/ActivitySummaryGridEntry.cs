using System.ComponentModel;

namespace Chronos.WinForms.DataObjects
{
    internal class ActivitySummaryGridEntry
    {
        [DisplayName("Activity")]
        public string ActivityName { get; }
        public TimeSpan Duration { get; }

        public ActivitySummaryGridEntry(string activityName, TimeSpan duration)
        {
            ActivityName = activityName;
            Duration = duration;
        }
    }
}
