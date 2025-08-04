using System.ComponentModel;

namespace Chronos.WinForms.DataObjects
{
    internal class DayStatisticsGridEntry
    {
        [DisplayName("Statistic")]
        public string StatisticName { get; }
        public TimeSpan Duration { get; }

        public DayStatisticsGridEntry(string statisticName, TimeSpan duration)
        {
            StatisticName = statisticName;
            Duration = duration;
        }
    }
}
