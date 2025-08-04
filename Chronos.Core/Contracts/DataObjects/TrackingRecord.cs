namespace Chronos.Core.Contracts.DataObjects
{
    public class TrackingRecord
    {
        public int Id { get; }
        public TimeOnly Start { get; }
        public TimeOnly End { get; }
        public TimeSpan Duration { get; }
        public int ActivityId { get; }
        public string ActivityName { get; }
        public int ObjectiveId { get; }
        public string ObjectiveName { get; }
        public bool IsActive { get; }

        public TrackingRecord(int recordId, TimeOnly start, TimeOnly end, TimeSpan duration, int activityId, string activityName, int objectiveId, string objectiveName, bool isActive)
        {
            Id = recordId;
            Start = start;
            End = end;
            Duration = duration;
            ActivityId = activityId;
            ActivityName = activityName;
            ObjectiveId = objectiveId;
            ObjectiveName = objectiveName;
            IsActive = isActive;
        }
    }
}
