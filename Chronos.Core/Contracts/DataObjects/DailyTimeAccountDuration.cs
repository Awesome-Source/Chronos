namespace Chronos.Core.Contracts.DataObjects
{
    public class DailyTimeAccountDuration
    {
        public DateOnly Date { get; }
        public int TimeAccountId { get; }
        public string TimeAccountName { get; }
        public bool IsWorkTime { get; }
        public TimeSpan Duration { get; }

        public DailyTimeAccountDuration(DateOnly date, int timeAccountId, string timeAccountName, bool isWorkTime, TimeSpan duration)
        {
            Date = date;
            TimeAccountId = timeAccountId;
            TimeAccountName = timeAccountName;
            IsWorkTime = isWorkTime;
            Duration = duration;
        }
    }
}
