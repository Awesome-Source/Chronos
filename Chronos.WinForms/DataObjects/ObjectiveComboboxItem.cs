using Chronos.Core.Contracts.DataObjects;

namespace Chronos.WinForms.DataObjects
{
    internal class ObjectiveComboboxItem
    {
        public int InternalId { get; }
        public string Name { get; }

        public ObjectiveComboboxItem(Objective objective)
        {
            InternalId = objective.Id;
            Name = objective.Name;
        }

        public override bool Equals(object? obj)
        {
            return obj is ObjectiveComboboxItem objectiveComboboxItem &&
                   InternalId == objectiveComboboxItem.InternalId;
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
