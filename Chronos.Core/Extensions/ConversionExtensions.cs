namespace Chronos.Core.Extensions
{
    internal static class ConversionExtensions
    {
        internal static int ToIntRepresentation(this bool value)
        {
            return value ? 1 : 0;
        }

        internal static bool ToBoolFromIntRepresentation(this int value)
        {
            return value == 1;
        }

        internal static int ToSecondsSinceMidnight(this TimeOnly timeOnly)
        {
            return (int) timeOnly.ToTimeSpan().TotalSeconds;
        }

        internal static TimeOnly FromSecondsSinceMidnight(this int secondsSinceMidnight)
        {
            return TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(secondsSinceMidnight));
        }
    }
}
