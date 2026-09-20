using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Services
{
    public interface IStatisticsService
    {
        IReadOnlyList<RelativeTimeAccountBalance> RetrieveAllProductiveTimeAccountBalances();

        /// <summary>Productive time per account for the week (Monday start) that contains <paramref name="today"/>, up to and including <paramref name="today"/>.</summary>
        IReadOnlyList<RelativeTimeAccountBalance> RetrieveCurrentWeekProductiveTimeAccountBalances(DateOnly today);

        /// <summary>One entry per day from Monday of the week containing <paramref name="today"/> up to and including <paramref name="today"/>.</summary>
        IReadOnlyList<DailyTimeAccountBreakdown> RetrieveCurrentWeekDailyBreakdown(DateOnly today);
    }
}
