namespace Chronos.Core.Contracts.DataObjects
{
    public class Objective
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }

        public Objective(int internalId, string name, string description)
        {
            Id = internalId;
            Name = name;
            Description = description;
        }        
    }
}
