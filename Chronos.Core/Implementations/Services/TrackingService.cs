using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Contracts.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Chronos.Core.Implementations.Services
{
    internal class TrackingService : ITrackingService
    {
        private readonly ITrackingTargetRepository _trackingTargetRepository;
        private readonly ITrackingDayRepository _trackingDayRepository;
        private readonly ITrackingRecordRepository _trackingRecordRepository;

        public TrackingService(ITrackingTargetRepository trackingTargetRepository, ITrackingDayRepository trackingDayRepository, ITrackingRecordRepository trackingRecordRepository)
        {
            _trackingTargetRepository = trackingTargetRepository;
            _trackingDayRepository = trackingDayRepository;
            _trackingRecordRepository = trackingRecordRepository;
        }

        public int CreateTarget(DateOnly date, int activityId, int objectiveId, bool isPlannedActivity)
        {
            var trackingDayId = GetOrCreateTrackingDay(date);

            return _trackingTargetRepository.Create(trackingDayId, activityId, objectiveId, isPlannedActivity);
        }

        public IReadOnlyList<EvaluatedTrackingTarget> GetEvaluatedTrackingTargetsForDay(DateTime now)
        {
            var trackingDayId = GetOrCreateTrackingDay(DateOnly.FromDateTime(now));
            var evaluationTime = TimeOnly.FromDateTime(now);

            return _trackingTargetRepository.GetEvaluatedTrackingTargetsForDay(trackingDayId, evaluationTime);
        }

        private int GetOrCreateTrackingDay(DateOnly date)
        {
            return _trackingDayRepository.GetOrCreateTrackingDay(date);
        }

        public IReadOnlyList<TrackingRecord> GetRecordsForDay(DateOnly date)
        {
            var trackingDayId = _trackingDayRepository.GetOrCreateTrackingDay(date);

            return _trackingRecordRepository.GetRecordsForDay(trackingDayId);
        }

        public IReadOnlyList<EvaluatedTrackingTarget> GetTimeSheetForDay(DateOnly date)
        {
            var trackingDayId = _trackingDayRepository.GetOrCreateTrackingDay(date);

            return _trackingTargetRepository.GetEvaluatedTrackingTargetsForDay(trackingDayId);
        }

        public void StartTracking(int trackingTargetId, TimeOnly start)
        {
            _trackingRecordRepository.CompleteActiveRecordAndStartNew(trackingTargetId, start);
        }

        public void StopTracking(TimeOnly end)
        {
            _trackingRecordRepository.CompleteActiveRecord(end);
        }

        public void UpdateRecord(int recordId, TimeOnly start, TimeOnly end)
        {
            _trackingRecordRepository.UpdateRecord(recordId, start, end);
        }

        public void CompleteActiveEntryInPastIfExisting(DateOnly date)
        {
            var trackingDayId = _trackingDayRepository.GetOrCreateTrackingDay(date);

            _trackingRecordRepository.CompleteActiveEntryInPastIfExisting(trackingDayId);
        }
    }
}
