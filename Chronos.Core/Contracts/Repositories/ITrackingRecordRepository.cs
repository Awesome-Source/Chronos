using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Repositories
{
    internal interface ITrackingRecordRepository
    {
        void CompleteActiveEntryInPastIfExisting(int currentTrackingDayId);
        void CompleteActiveRecord(TimeOnly end);
        void CompleteActiveRecordAndStartNew(int targetId, TimeOnly start);
        IReadOnlyList<TrackingRecord> GetRecordsForDay(int trackingDayId);
        void UpdateRecord(int recordId, TimeOnly start, TimeOnly end);
    }
}
