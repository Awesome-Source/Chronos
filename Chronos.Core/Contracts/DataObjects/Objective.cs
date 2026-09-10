namespace Chronos.Core.Contracts.DataObjects
{
    public class Objective
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public string CategoryName { get; }
        public int CategoryId { get; set; }

        public Objective(int internalId, string name, string description, string categoryName, int categoryId)
        {
            Id = internalId;
            Name = name;
            Description = description;
            CategoryName = categoryName;
            CategoryId = categoryId;
        }        
    }
}
