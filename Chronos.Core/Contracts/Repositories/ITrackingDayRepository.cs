namespace Chronos.Core.Contracts.Repositories
{
    internal interface ITrackingDayRepository
    {
        int GetOrCreateTrackingDay(DateOnly date);
    }
}
