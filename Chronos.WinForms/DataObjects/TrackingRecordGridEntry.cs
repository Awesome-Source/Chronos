using System.ComponentModel;
using Chronos.Core.Contracts.DataObjects;
using Chronos.WinForms.Attributes;

namespace Chronos.WinForms.DataObjects
{
    internal class TrackingRecordGridEntry
    {
        public int Id { get; }
        public TimeOnly Start { get; }
        public TimeOnly End { get; }
        public TimeSpan Duration { get; }

        [HiddenColumn]
        public int ActivityId { get; }

        [DisplayName("Activity")]
        public string ActivityName { get; }

        [HiddenColumn]
        public int ObjectiveId { get; }

        [DisplayName("Objective")]
        public string ObjectiveName { get; }

        [DisplayName("Is active")]
        public bool IsActive { get; }

        public TrackingRecordGridEntry(TrackingRecord record)
        {
            Id = record.Id;
            Start = record.Start;
            End = record.End;
            Duration = record.Duration;
            ActivityId = record.ActivityId;
            ActivityName = record.ActivityName;
            ObjectiveId = record.ObjectiveId;
            ObjectiveName = record.ObjectiveName;
            IsActive = record.IsActive;
        }
    }
}
