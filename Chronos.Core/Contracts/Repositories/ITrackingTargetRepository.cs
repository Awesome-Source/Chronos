using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Repositories
{
    internal interface ITrackingTargetRepository
    {
        int Create(int trackingDayId, int activityId, int objectiveId, bool isPlannedActivity);
        IReadOnlyList<EvaluatedTrackingTarget> GetEvaluatedTrackingTargetsForDay(int trackingDayId);
        IReadOnlyList<EvaluatedTrackingTarget> GetEvaluatedTrackingTargetsForDay(int trackingDayId, TimeOnly evaluationTime);
    }
}
