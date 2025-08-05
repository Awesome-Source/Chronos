using Apollo.Core.Interfaces;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;

namespace Chronos.Core.Implementations.Repositories
{
    internal class StatisticsRepository : IStatisticsRepository
    {
        private readonly IDatabaseAccessor _databaseAccessor;

        public StatisticsRepository(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public IReadOnlyList<TimeAccountBalance> RetrieveAllTimeAccountBalances()
        {
            var sql = @"SELECT 
                            ta.id, 
                            ta.name, 
                            (
                                SELECT SUM(tr.duration) FROM tracking_records tr 
                                INNER JOIN tracking_targets tt ON tr.tracking_target_id = tt.id 
                                INNER JOIN activities a ON tt.activity_id = a.id 
                                WHERE a.time_account_id = ta.id
                            ) AS accumulated_duration
                        FROM time_accounts ta
                        ORDER BY accumulated_duration DESC";

            return _databaseAccessor.ExecuteQuery(sql, rp => new TimeAccountBalance(rp.GetInt("id"), rp.GetString("name"), TimeSpan.FromSeconds(rp.GetNullableInt("accumulated_duration") ?? 0)));
        }
    }
}
