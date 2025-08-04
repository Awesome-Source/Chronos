namespace Chronos.Core.Contracts.DataObjects
{
    internal class ActiveTrackingRecord
    {
        public int TrackingTargetId { get; }
        public TimeOnly Start { get; }

        public ActiveTrackingRecord(int trackingTargetId, TimeOnly start)
        {
            TrackingTargetId = trackingTargetId;
            Start = start;
        }
    }
}
