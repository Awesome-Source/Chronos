namespace Chronos.Core.Contracts.DataObjects
{
    public class EvaluatedTrackingTarget
    {
        public int InternalId { get; }
        public string ActivityName { get; }
        public string TimeAccountName { get; }
        public string ObjectiveName { get; }
        public bool IsPlannedActivity { get; }
        public TimeSpan AccumulatedTime { get; internal set; }
        public bool IsActive { get; internal set; }

        public EvaluatedTrackingTarget(int internalId, string activityName, string timeAccountName, string objectiveName, bool isPlannedActivity, TimeSpan accumulatedTime, bool isActive)
        {
            InternalId = internalId;
            ActivityName = activityName;
            TimeAccountName = timeAccountName;
            ObjectiveName = objectiveName;
            IsPlannedActivity = isPlannedActivity;
            AccumulatedTime = accumulatedTime;
            IsActive = isActive;
        }
    }
}
