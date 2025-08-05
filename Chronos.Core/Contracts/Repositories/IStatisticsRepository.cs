using Chronos.Core.Contracts.DataObjects;

namespace Chronos.Core.Contracts.Repositories
{
    public interface IStatisticsRepository
    {
        IReadOnlyList<TimeAccountBalance> RetrieveAllTimeBalances();
    }
}
