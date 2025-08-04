using System.ComponentModel;
using Chronos.Core.Contracts.DataObjects;
using Chronos.WinForms.Attributes;

namespace Chronos.WinForms.DataObjects
{
    internal class TrackingTargetGridEntry
    {
        [HiddenColumn]
        public int TrackingTargetId { get; }

        [DisplayName("Activity")]
        public string ActivityName { get; }

        [DisplayName("Objective")]
        public string ObjectiveName { get; }

        [DisplayName("Time Account")]
        public string TimeAccountName { get;  }

        [DisplayName("Is planned")]
        public bool IsPlannedActivity { get;  }

        [DisplayName("Accumulated time")]
        public TimeSpan AccumulatedTime { get;  }

        [HiddenColumn]
        public bool IsActive { get;  }

        public TrackingTargetGridEntry(EvaluatedTrackingTarget trackingTarget)
        {
            TrackingTargetId = trackingTarget.InternalId;
            ActivityName = trackingTarget.ActivityName;
            ObjectiveName = trackingTarget.ObjectiveName;
            IsPlannedActivity = trackingTarget.IsPlannedActivity;
            TimeAccountName = trackingTarget.TimeAccountName;
            AccumulatedTime = trackingTarget.AccumulatedTime;
            IsActive = trackingTarget.IsActive;
        }
    }
}
