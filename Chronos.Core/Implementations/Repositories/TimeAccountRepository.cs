using System.Drawing;
using Apollo.Core.Interfaces;
using Chronos.Core.Contracts.DataObjects;
using Chronos.Core.Contracts.Repositories;
using Chronos.Core.Extensions;

namespace Chronos.Core.Implementations.Repositories
{
    internal class TimeAccountRepository : ITimeAccountRepository
    {
        private readonly IDatabaseAccessor _databaseAccessor;

        public TimeAccountRepository(IDatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public int Add(string name, Color color, bool isWorkTime)
        {
            var parameters = new Dictionary<string, object>
            {
                {"NAME", name },
                {"COLOR", color.ToHexRepresentation() },
                {"WORKTIME", isWorkTime.ToIntRepresentation() },
            };

            return _databaseAccessor.ExecuteQuery("INSERT INTO time_accounts (name, color, is_worktime) VALUES (@NAME, @COLOR, @WORKTIME) RETURNING id", rp => rp.GetInt("id"), parameters).Single();
        }

        public IReadOnlyList<TimeAccount> GetValues()
        {
            return _databaseAccessor.ExecuteQuery("SELECT id, name, color, is_worktime from time_accounts", ParseTimeAccount);
        }

        private static TimeAccount ParseTimeAccount(IRowParser rowParser)
        {
            return new TimeAccount(rowParser.GetInt("id"), rowParser.GetString("name"), rowParser.GetString("color").ToColorFromHexRepresentation(), rowParser.GetInt("is_worktime").ToBoolFromIntRepresentation());
        }

        public void Remove(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", id}
            };

            _databaseAccessor.ExecuteNonQuery("DELETE FROM time_accounts WHERE id = @ID", parameters);
        }

        public void Update(int id, string name, Color color, bool isWorkTime)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID", id },
                {"NAME", name },
                {"COLOR", color.ToHexRepresentation() },
                {"WORKTIME", isWorkTime.ToIntRepresentation() },
            };

            _databaseAccessor.ExecuteNonQuery("UPDATE time_accounts SET name = @NAME, color = @COLOR, is_worktime = @WORKTIME WHERE id = @ID", parameters);
        }
    }
}
