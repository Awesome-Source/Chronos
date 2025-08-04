using Chronos.Core.Contracts.DataObjects;

namespace Chronos.WinForms.DataObjects
{
    internal class TimeAccountComboboxItem
    {
        public int InternalId { get; }
        public string Name { get; }

        public TimeAccountComboboxItem(TimeAccount timeAccount)
        {
            InternalId = timeAccount.Id;
            Name = timeAccount.Name;
        }

        public override bool Equals(object? obj)
        {
            return obj is TimeAccountComboboxItem account &&
                   InternalId == account.InternalId;
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
