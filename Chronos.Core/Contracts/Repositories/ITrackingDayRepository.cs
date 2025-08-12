using System.Diagnostics.CodeAnalysis;

namespace Chronos.Core.Contracts.Repositories
{
    internal interface ITrackingDayRepository
    {
        int GetOrCreateTrackingDay(DateOnly date);
        bool TryGetTrackingDay(DateOnly date, out int trackingDayId);
        bool TryGetLatestTrackingDayBefore(DateOnly date, out DateOnly latestDateBefore);
    }
}
