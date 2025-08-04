using Chronos.Core.Contracts.DataObjects;

namespace Chronos.WinForms.DataObjects
{
    internal class ActivityComboboxItem
    {
        public int InternalId { get; }
        public string Name { get; }

        public ActivityComboboxItem(Activity activity)
        {
            InternalId = activity.Id;
            Name = activity.Name;
        }

        public override bool Equals(object? obj)
        {
            return obj is ActivityComboboxItem activityComboboxItem &&
                   InternalId == activityComboboxItem.InternalId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InternalId);
        }

        public override string ToString()
        {
            return $"{InternalId} {Name}";
        }
    }
}
