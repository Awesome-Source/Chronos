using System.ComponentModel;
using Chronos.Core.Contracts.DataObjects;

namespace Chronos.WinForms.DataObjects
{
    internal class ObjectiveGridEntry
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ObjectiveGridEntry(Objective objective)
        {
            Id = objective.Id;
            Name = objective.Name;
            Description = objective.Description;
        }        

        public override bool Equals(object? obj)
        {
            return obj is ObjectiveGridEntry objective &&
                   Id == objective.Id;
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
