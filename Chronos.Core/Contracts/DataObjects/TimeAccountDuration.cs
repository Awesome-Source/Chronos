namespace Chronos.Core.Contracts.DataObjects
{
    public class TimeAccountDuration
    {
        public int TimeAccountId { get; }
        public string TimeAccountName { get; }
        public bool IsWorkTime { get; }
        public TimeSpan Duration { get; }

        public TimeAccountDuration(int timeAccountId, string timeAccountName, bool isWorkTime, TimeSpan duration)
        {
            TimeAccountId = timeAccountId;
            TimeAccountName = timeAccountName;
            IsWorkTime = isWorkTime;
            Duration = duration;
        }
    }
}
