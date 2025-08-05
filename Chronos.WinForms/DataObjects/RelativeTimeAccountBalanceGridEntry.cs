using System.ComponentModel;

namespace Chronos.WinForms.DataObjects
{
    internal class RelativeTimeAccountBalanceGridEntry
    {
        [DisplayName("Time Account")]
        public string TimeAccountName { get; }
        public TimeSpan Duration { get; }
        public double Percentage { get; }

        public RelativeTimeAccountBalanceGridEntry(string timeAccountName, TimeSpan duration, double percentage)
        {
            TimeAccountName = timeAccountName;
            Duration = duration;
            Percentage = percentage;
        }
    }
}
