using Chronos.Core.Contracts.DataObjects;

namespace Chronos.WinForms.DataObjects
{
    internal class CategoryComboboxItem
    {
        public int InternalId { get; }
        public string Name { get; }

        public CategoryComboboxItem(Category category)
        {
            InternalId = category.Id;
            Name = category.Name;
        }

        public override bool Equals(object? obj)
        {
            return obj is CategoryComboboxItem account &&
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
