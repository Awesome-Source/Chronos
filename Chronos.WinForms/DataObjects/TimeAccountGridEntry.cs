using System.ComponentModel;
using Chronos.Core.Contracts.DataObjects;
using Chronos.WinForms.Attributes;
using Chronos.WinForms.Views.Common;

namespace Chronos.WinForms.DataObjects
{
    public class TimeAccountGridEntry
    {
        public int Id { get; }
        public string Name { get; set; }

        [CellTemplateType(typeof(ColorGridCell))]
        public Color Color { get; set; }

        [DisplayName("Work time")]
        public bool IsWorkTime { get; set; }

        public TimeAccountGridEntry(TimeAccount timeAccount)
        {
            Id = timeAccount.Id;
            Name = timeAccount.Name;
            Color = timeAccount.Color;
            IsWorkTime = timeAccount.IsWorkTime;
        }

        public override bool Equals(object? obj)
        {
            return obj is TimeAccountGridEntry account &&
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
