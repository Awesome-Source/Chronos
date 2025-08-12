using System.Diagnostics.CodeAnalysis;
using Apollo.Core.Interfaces;
using Chronos.Core.Contracts.Repositories;

namespace Chronos.Core.Implementations.Repositories
{
    internal class TrackingDayRepository : ITrackingDayRepository
    {
        private readonly IDatabaseAccessor _databaseAccessor;

        public TrackingDayRepository(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public int GetOrCreateTrackingDay(DateOnly date)
        {
            if(TryGetTrackingDay(date, out var existingTrackingDayId))
            {
                return existingTrackingDayId;
            }

            return CreateTrackingDay(date);
        }

        private int CreateTrackingDay(DateOnly date)
        {
            var parameters = new Dictionary<string, object>
            {
                {"YEAR", date.Year },
                {"MONTH", date.Month },
                {"DAY", date.Day }
            };

            return _databaseAccessor.ExecuteQuery("INSERT INTO tracking_days (year, month, day) VALUES (@YEAR, @MONTH, @DAY) RETURNING id", rp => rp.GetInt("id"), parameters).Single();
        }

        public bool TryGetTrackingDay(DateOnly date, out int trackingDayId)
        {
            var parameters = new Dictionary<string, object>
            {
                {"YEAR", date.Year },
                {"MONTH", date.Month },
                {"DAY", date.Day }
            };

            var ids = _databaseAccessor.ExecuteQuery("SELECT id FROM tracking_days WHERE year = @YEAR AND month = @MONTH AND day = @DAY", rp => rp.GetInt("id"), parameters);
            if (ids.Count == 0)
            {
                trackingDayId = -1;
                return false;
                
            }

            trackingDayId = ids.Single();
            return true;
        }

        public bool TryGetLatestTrackingDayBefore(DateOnly date, out DateOnly latestDateBefore)
        {
            var parameters = new Dictionary<string, object>
            {
                {"YEAR", date.Year },
                {"MONTH", date.Month },
                {"DAY", date.Day }
            };

            var dates = _databaseAccessor.ExecuteQuery("SELECT year, month, day FROM tracking_days WHERE year <> @YEAR OR month <> @MONTH OR day <> @DAY ORDER BY id DESC LIMIT 1", ParseDateOnly, parameters);
            if (dates.Count == 0)
            {
                latestDateBefore = DateOnly.MinValue;
                return false;

            }

            latestDateBefore = dates.Single();
            return true;
        }

        private DateOnly ParseDateOnly(IRowParser rowParser)
        {
            return new DateOnly(rowParser.GetInt("year"), rowParser.GetInt("month"), rowParser.GetInt("day"));
        }
    }
}
