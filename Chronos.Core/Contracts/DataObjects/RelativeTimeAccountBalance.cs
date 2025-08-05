namespace Chronos.Core.Contracts.DataObjects
{
    public class RelativeTimeAccountBalance
    {
        public int TimeAccountId { get; }
        public string TimeAccountName { get; }
        public TimeSpan AccumulatedDuration { get; }
        public double Proportion { get; }

        public RelativeTimeAccountBalance(int timeAccountId, string timeAccountName, TimeSpan accumulatedDuration, double proportion)
        {
            TimeAccountId = timeAccountId;
            TimeAccountName = timeAccountName;
            AccumulatedDuration = accumulatedDuration;
            Proportion = proportion;
        }
    }
}
