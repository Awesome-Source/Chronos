using System.Drawing;

namespace Chronos.Core.Extensions
{
    internal static class ConversionExtensions
    {
        internal static string ToHexRepresentation(this Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        internal static Color ToColorFromHexRepresentation(this string hexRepresentation)
        {
            return ColorTranslator.FromHtml(hexRepresentation);
        }

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
