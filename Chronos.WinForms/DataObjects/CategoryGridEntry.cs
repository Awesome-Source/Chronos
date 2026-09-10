using Chronos.Core.Contracts.DataObjects;

namespace Chronos.WinForms.DataObjects
{
    internal class CategoryGridEntry
    {
        public int Id { get; }
        public string Name { get; set; }

        public CategoryGridEntry(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }

        public override bool Equals(object? obj)
        {
            return obj is CategoryGridEntry category &&
                   Id == category.Id;
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
