using Apollo.Core.Interfaces;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Extensions;

namespace Chronos.Core.Implementations.Repositories
{
    internal class StatisticsRepository : IStatisticsRepository
    {
        private readonly IDatabaseAccessor _databaseAccessor;

        public StatisticsRepository(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public IReadOnlyList<TimeAccountBalance> RetrieveAllProductiveTimeAccountBalances()
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
                        WHERE ta.is_worktime = 1
                        ORDER BY accumulated_duration DESC";

            return _databaseAccessor.ExecuteQuery(sql, rp => new TimeAccountBalance(rp.GetInt("id"), rp.GetString("name"), TimeSpan.FromSeconds(rp.GetNullableInt("accumulated_duration") ?? 0)));
        }

        public IReadOnlyList<DailyTimeAccountDuration> RetrieveDailyTimeAccountDurations(DateOnly from, DateOnly to)
        {
            var parameters = new Dictionary<string, object>
            {
                {"FROM", ToYyyyMmDd(from) },
                {"TO", ToYyyyMmDd(to) }
            };

            var sql = @"SELECT
                            td.year,
                            td.month,
                            td.day,
                            ta.id,
                            ta.name,
                            ta.is_worktime,
                            SUM(tr.duration) AS accumulated_duration
                        FROM tracking_records tr
                        INNER JOIN tracking_targets tt ON tr.tracking_target_id = tt.id
                        INNER JOIN tracking_days td ON tt.tracking_day_id = td.id
                        INNER JOIN activities a ON tt.activity_id = a.id
                        INNER JOIN time_accounts ta ON a.time_account_id = ta.id
                        WHERE (td.year * 10000 + td.month * 100 + td.day) > @FROM AND (td.year * 10000 + td.month * 100 + td.day) < @TO
                        GROUP BY td.year, td.month, td.day, ta.id, ta.name, ta.is_worktime";

            return _databaseAccessor.ExecuteQuery(sql, ParseDailyTimeAccountDuration, parameters);
        }

        private static int ToYyyyMmDd(DateOnly date)
        {
            return date.Year * 10000 + date.Month * 100 + date.Day;
        }

        private static DailyTimeAccountDuration ParseDailyTimeAccountDuration(IRowParser rowParser)
        {
            return new DailyTimeAccountDuration(
                new DateOnly(rowParser.GetInt("year"), rowParser.GetInt("month"), rowParser.GetInt("day")),
                rowParser.GetInt("id"),
                rowParser.GetString("name"),
                rowParser.GetInt("is_worktime").ToBoolFromIntRepresentation(),
                TimeSpan.FromSeconds(rowParser.GetInt("accumulated_duration")));
        }
    }
}
