namespace Chronos.Core.Contracts.DataObjects
{
    public class DailyTimeAccountBreakdown
    {
        public DateOnly Date { get; }
        public TimeSpan TotalWorkTime { get; }
        public IReadOnlyList<TimeAccountDuration> Accounts { get; }

        public DailyTimeAccountBreakdown(DateOnly date, TimeSpan totalWorkTime, IReadOnlyList<TimeAccountDuration> accounts)
        {
            Date = date;
            TotalWorkTime = totalWorkTime;
            Accounts = accounts;
        }
    }
}
