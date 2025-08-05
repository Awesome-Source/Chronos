namespace Chronos.Core.Contracts.DataObjects
{
    public class TimeAccountBalance
    {
        public int TimeAccountId { get; }
        public string TimeAccountName { get; }
        public TimeSpan AccumulatedDuration { get; }

        public TimeAccountBalance(int timeAccountId, string timeAccountName, TimeSpan accumulatedDuration)
        {
            TimeAccountId = timeAccountId;
            TimeAccountName = timeAccountName;
            AccumulatedDuration = accumulatedDuration;
        }
    }
}
