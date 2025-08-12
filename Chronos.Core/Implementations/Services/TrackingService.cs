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
            var trackingDayId = _trackingDayRepository.GetOrCreateTrackingDay(date);

            return _trackingTargetRepository.Create(trackingDayId, activityId, objectiveId, isPlannedActivity);
        }

        public IReadOnlyList<EvaluatedTrackingTarget> GetEvaluatedTrackingTargetsForDay(DateTime evaluationTimeStamp)
        {
            if(!_trackingDayRepository.TryGetTrackingDay(DateOnly.FromDateTime(evaluationTimeStamp), out var trackingDayId))
            {
                return new List<EvaluatedTrackingTarget>();
            }

            var evaluationTime = TimeOnly.FromDateTime(evaluationTimeStamp);

            return _trackingTargetRepository.GetEvaluatedTrackingTargetsForDay(trackingDayId, evaluationTime);
        }

        public IReadOnlyList<TrackingRecord> GetRecordsForDay(DateOnly date)
        {
            if (!_trackingDayRepository.TryGetTrackingDay(date, out var trackingDayId))
            {
                return new List<TrackingRecord>();
            }

            return _trackingRecordRepository.GetRecordsForDay(trackingDayId);
        }

        public IReadOnlyList<EvaluatedTrackingTarget> GetTimeSheetForDay(DateOnly date)
        {
            if (!_trackingDayRepository.TryGetTrackingDay(date, out var trackingDayId))
            {
                return new List<EvaluatedTrackingTarget>();
            }

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
            if (!_trackingDayRepository.TryGetTrackingDay(date, out var trackingDayId))
            {
                return;
            }

            _trackingRecordRepository.CompleteActiveEntryInPastIfExisting(trackingDayId);
        }
    }
}
