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

        public IReadOnlyList<RelativeTimeAccountBalance> RetrieveAllTimeBalances()
        {
            var balances = _statisticsRepository.RetrieveAllTimeBalances();
            var totalDurationInSeconds = balances.Select(b => b.AccumulatedDuration.TotalSeconds).Sum();

            return balances
                .Select(b => new RelativeTimeAccountBalance(b.TimeAccountId, b.TimeAccountName, b.AccumulatedDuration, b.AccumulatedDuration.TotalSeconds / totalDurationInSeconds))
                .ToList();
        }
    }
}
