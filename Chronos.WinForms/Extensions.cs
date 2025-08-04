using System.ComponentModel;

namespace Chronos.WinForms
{
    public static class Extensions
    {

        public static string FormatTimeSpan(this TimeSpan ts)
        {
            return $"{ts.Hours} h {ts.Minutes} min {ts.Seconds} sec";
        }

        public static void AddRange<T>(this BindingList<T> bindingList, IEnumerable<T> values)
        {
            foreach(var value in values)
            {
                bindingList.Add(value);
            }
        }
    }
}
