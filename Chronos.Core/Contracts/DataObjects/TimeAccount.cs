namespace Chronos.Core.Contracts.DataObjects
{
    public class TimeAccount
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool IsWorkTime { get; set; }

        public TimeAccount(int internalId, string name, string color, bool isWorkTime)
        {
            Id = internalId;
            Name = name;
            Color = color;
            IsWorkTime = isWorkTime;
        }

        public override bool Equals(object? obj)
        {
            return obj is TimeAccount account &&
                   Id == account.Id;
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
