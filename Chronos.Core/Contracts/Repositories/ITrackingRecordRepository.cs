using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Repositories
{
    internal interface ITrackingRecordRepository
    {
        void AddRecord(int targetId, TimeOnly start, TimeOnly end);
        void CompleteActiveEntryInPastIfExisting(int currentTrackingDayId);
        void CompleteActiveRecord(TimeOnly end);
        void CompleteActiveRecordAndStartNew(int targetId, TimeOnly start);
        void DeleteRecord(int recordId);
        IReadOnlyList<TrackingRecord> GetRecordsForDay(int trackingDayId);
        void UpdateRecord(int recordId, TimeOnly start, TimeOnly end);
    }
}
