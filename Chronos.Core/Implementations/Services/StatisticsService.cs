using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Contracts.Services;

namespace Chronos.Core.Implementations.Services
{
    internal class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;

        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }

        public IReadOnlyList<RelativeTimeAccountBalance> RetrieveAllProductiveTimeAccountBalances()
        {
            return ToRelativeBalances(_statisticsRepository.RetrieveAllProductiveTimeAccountBalances());
        }

        public IReadOnlyList<RelativeTimeAccountBalance> RetrieveCurrentWeekProductiveTimeAccountBalances(DateOnly today)
        {
            var durations = _statisticsRepository.RetrieveDailyTimeAccountDurations(StartOfWeek(today), today);

            var balances = durations
                .Where(d => d.IsWorkTime)
                .GroupBy(d => new { d.TimeAccountId, d.TimeAccountName })
                .Select(g => new TimeAccountBalance(g.Key.TimeAccountId, g.Key.TimeAccountName, TimeSpan.FromSeconds(g.Sum(d => d.Duration.TotalSeconds))))
                .OrderByDescending(b => b.AccumulatedDuration)
                .ToList();

            return ToRelativeBalances(balances);
        }

        public IReadOnlyList<DailyTimeAccountBreakdown> RetrieveCurrentWeekDailyBreakdown(DateOnly today)
        {
            var startOfWeek = StartOfWeek(today);
            var durationsByDate = _statisticsRepository.RetrieveDailyTimeAccountDurations(startOfWeek, today)
                .ToLookup(d => d.Date);

            return Enumerable.Range(0, today.DayNumber - startOfWeek.DayNumber + 1)
                .Select(startOfWeek.AddDays)
                .Select(date =>
                {
                    var accounts = durationsByDate[date]
                        .Select(d => new TimeAccountDuration(d.TimeAccountId, d.TimeAccountName, d.IsWorkTime, d.Duration))
                        .OrderByDescending(a => a.Duration)
                        .ToList();
                    var totalWorkTime = TimeSpan.FromSeconds(accounts.Where(a => a.IsWorkTime).Sum(a => a.Duration.TotalSeconds));

                    return new DailyTimeAccountBreakdown(date, totalWorkTime, accounts);
                })
                .ToList();
        }

        /// <summary>Weeks start on Monday.</summary>
        private static DateOnly StartOfWeek(DateOnly date)
        {
            var daysSinceMonday = ((int)date.DayOfWeek + 6) % 7;
            return date.AddDays(-daysSinceMonday);
        }

        private static IReadOnlyList<RelativeTimeAccountBalance> ToRelativeBalances(IReadOnlyList<TimeAccountBalance> balances)
        {
            var totalDurationInSeconds = balances.Select(b => b.AccumulatedDuration.TotalSeconds).Sum();

            return balances
                .Select(b => new RelativeTimeAccountBalance(b.TimeAccountId, b.TimeAccountName, b.AccumulatedDuration, totalDurationInSeconds > 0 ? b.AccumulatedDuration.TotalSeconds / totalDurationInSeconds : 0))
                .ToList();
        }
    }
}
