namespace Chronos.Core.Contracts.DataObjects
{
    public class Activity
    {
        public int Id { get; }
        public string Name { get; set; }
        public int TimeAccountId { get; set; }
        public string TimeAccountName { get; }

        public Activity(int id, string name, int timeAccountId, string timeAccountName)
        {
            Id = id;
            Name = name;
            TimeAccountId = timeAccountId;
            TimeAccountName = timeAccountName;
        }

        public override bool Equals(object? obj)
        {
            return obj is Activity activity &&
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
