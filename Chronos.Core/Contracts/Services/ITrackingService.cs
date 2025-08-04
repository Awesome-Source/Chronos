using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Services
{
    public interface ITrackingService
    {
        int CreateTarget(DateOnly date, int activityId, int objectiveId, bool isPlannedActivity);
        IReadOnlyList<EvaluatedTrackingTarget> GetEvaluatedTrackingTargetsForDay(DateTime now);
        void StartTracking(int targetId, TimeOnly start);
        void StopTracking(TimeOnly end);
        IReadOnlyList<TrackingRecord> GetRecordsForDay(DateOnly date);
        IReadOnlyList<EvaluatedTrackingTarget> GetTimeSheetForDay(DateOnly date);
        void UpdateRecord(int recordId, TimeOnly start, TimeOnly end);
        void CompleteActiveEntryInPastIfExisting(DateOnly date);
    }
}
