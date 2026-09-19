using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Services
{
    public interface ITrackingService
    {
        int CreateTarget(DateOnly date, int activityId, int objectiveId, bool isPlannedActivity);
        IReadOnlyList<EvaluatedTrackingTarget> GetEvaluatedTrackingTargetsForDay(DateTime evaluationTimeStamp);
        void StartTracking(int targetId, TimeOnly start);
        void StopTracking(TimeOnly end);
        IReadOnlyList<TrackingRecord> GetRecordsForDay(DateOnly date);
        IReadOnlyList<EvaluatedTrackingTarget> GetTimeSheetForDay(DateOnly date);
        IReadOnlyList<EvaluatedTrackingTarget> GetTrackingTargetsForDay(DateOnly date);
        void AddRecord(int trackingTargetId, TimeOnly start, TimeOnly end);
        void UpdateRecord(int recordId, TimeOnly start, TimeOnly end);
        void RemoveRecord(int recordId);
        void CompleteActiveEntryInPastIfExisting(DateOnly date);
        bool TryGetLatestTrackingDayBefore(DateOnly date, out DateOnly latestDateBefore);
    }
}
