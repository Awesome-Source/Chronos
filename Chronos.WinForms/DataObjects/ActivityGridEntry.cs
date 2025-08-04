using System.ComponentModel;
using Chronos.Core.Contracts.DataObjects;
using Chronos.WinForms.Attributes;

namespace Chronos.WinForms.DataObjects
{
    internal class ActivityGridEntry
    {
        public int Id { get; }
        public string Name { get; set; }

        [HiddenColumn]
        public int TimeAccountId { get; set; }

        [DisplayName("Time Account")]
        public string TimeAccountName { get; set; }

        public ActivityGridEntry(Activity activity)
        {
            Id = activity.Id;
            Name = activity.Name;
            TimeAccountId = activity.TimeAccountId;
            TimeAccountName = activity.TimeAccountName;
        }

        public override bool Equals(object? obj)
        {
            return obj is ActivityGridEntry activity &&
                   Id == activity.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return $"{Id} {Name}";
        }
    }
}
