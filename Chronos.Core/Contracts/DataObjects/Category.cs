namespace Chronos.Core.Contracts.DataObjects
{
    public class Category
    {
        public int Id { get; }
        public string Name { get; }

        public Category(int internalId, string name)
        {
            Id = internalId;
            Name = name;
        }
    }
}
