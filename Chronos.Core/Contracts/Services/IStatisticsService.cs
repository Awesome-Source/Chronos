using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Services
{
    public interface IStatisticsService
    {
        IReadOnlyList<RelativeTimeAccountBalance> RetrieveAllProductiveTimeAccountBalances();
    }
}
